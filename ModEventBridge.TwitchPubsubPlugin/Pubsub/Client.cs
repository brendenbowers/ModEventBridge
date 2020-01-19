using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using ModEventBridge.Plugin.EventSource;
using System.Net.WebSockets;
using System.Threading;
using ModEventBridge.TwitchPubsubPlugin.Pubsub.Messages;
using System.Collections.Concurrent;
using System.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.ChannelPoints;
using ModEventBridge.TwitchPubsubPlugin.Pubsub.Transforms;
using ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.Bits;
using ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.Subscription;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub
{
    public class PubsubClient
    {

        protected ConcurrentDictionary<string,List<string>> userIDs = new ConcurrentDictionary<string, List<string>>();
        protected Channel<Event> channel;
        protected ClientWebSocket client;
        protected CancellationTokenSource cts;
        protected Thread readerThread;
        protected ConcurrentDictionary<string, ListenRequest> pendingListenReqs;

        protected DateTime lastPong = DateTime.MinValue;
        protected int pingsSincePong = 0;

        // The interval to ping in (default 4 mins)
        public TimeSpan PingInterval { get; set; } = TimeSpan.FromMinutes(4);


        public ChannelReader<Event> Events => channel?.Reader;

        public Uri Target { get; protected set; }

        public ICollection<string> UserIDs => userIDs.Keys;

        public PubsubClient(Uri uri, BoundedChannelOptions channelOpts = default)
        {
            client = new ClientWebSocket();
            Target = uri;
            channel = Channel.CreateBounded<Event>(channelOpts);
        }


        protected async void ReadWorker(object data)
        {
            try
            {
                var ct = CancellationToken.None;
                if (data is CancellationToken)
                {
                    ct = (CancellationToken)data;
                }

                while (!ct.IsCancellationRequested)
                {
                    switch (client.State)
                    {
                        case WebSocketState.Connecting:
                            Thread.Sleep(1000);
                            continue;
                        case WebSocketState.Open:
                            break;
                        default:
                            return;

                    }

                    var buffer = new ArraySegment<byte>();
                    var readTask = client.ReceiveAsync(buffer, ct);
                    // use wait any to wait the task or timeout to a ping interval
                    var waitRes = Task.WaitAny(new Task[] { readTask }, (int)PingInterval.TotalMilliseconds, ct);
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }
                    
                    if(waitRes ==- 1 || lastPong.Add(PingInterval).AddSeconds(20) < DateTime.UtcNow)
                    {
                        pingsSincePong++;
                        await SendJson(Message.Ping);
                        if(!readTask.IsCompleted)
                        {
                            readTask.Dispose();
                            continue;
                        }
                    }

                    if (readTask.Result.CloseStatus != null)
                    {
                        // handle close
                        return;
                    }

                    var msg = FromArraySegement<Message>(buffer);
                    switch(msg.Type)
                    {
                        case "PONG":
                            lastPong = DateTime.UtcNow;
                            pingsSincePong = 0;
                            continue;
                        case "RESPONSE":
                            HandleResponseMessage(buffer);
                            continue;
                        case "MESSAGE":
                            await HandleMessage(buffer);
                            continue;
                        case "reward-redeemed":
                            await HandleRewardReedemed(buffer);
                            continue;
                        //default:
                            //unhandled, log message
                    }

                    if(pingsSincePong > 5)
                    {
                        // too many unanswered pings
                        return;
                    }
                }
            } catch (Exception ex)
            {
                // todo: log this and do something
            }


        }

        protected void HandleResponseMessage(ArraySegment<byte> buffer)
        {
            var response = FromArraySegement<ListenResponse>(buffer);
            if(response.Error == "")
            {
                var found = pendingListenReqs.FirstOrDefault((kv) => kv.Value.Nonce == response.Nonce);
                if(found.Key == null && found.Value == null)
                {
                    // log and do somethning
                    return;
                }

                pendingListenReqs.TryRemove(found.Key, out _);
                userIDs.TryAdd(found.Key, found.Value.Data.Topics);
            }
            // handle error
        }

        protected ValueTask HandleMessage(ArraySegment<byte> buffer)
        {
            var dataMsg = FromArraySegement<TopicMessage>(buffer);
            if(dataMsg.Message.Topic.StartsWith("channel-bits-events-v2"))
            {
                return HandleBitsMessage(dataMsg.Message.Message);
            }

            if(dataMsg.Message.Topic.StartsWith("channel-subscribe-events-v1"))
            {
                return HandleSubMessage(dataMsg.Message.Message);
            }

            // unhandled type, add logging
            return new ValueTask(Task.CompletedTask);
        }

        protected async ValueTask HandleRewardReedemed(ArraySegment<byte> buffer)
        {
           var evt = FromArraySegement<Points>(buffer).ToEvent();

            await channel.Writer.WaitToWriteAsync(cts.Token);
            await channel.Writer.WriteAsync(evt, cts.Token);
        }

        protected async ValueTask HandleBitsMessage(string data)
        {
            var evt = JsonConvert.DeserializeObject<Bits>(data).ToEvent(data);

            await channel.Writer.WaitToWriteAsync(cts.Token);
            await channel.Writer.WriteAsync(evt, cts.Token);
        }

        protected async ValueTask HandleSubMessage(string data)
        {
            var evt = JsonConvert.DeserializeObject<Subscription>(data).ToEvent(data);

            await channel.Writer.WaitToWriteAsync(cts.Token);
            await channel.Writer.WriteAsync(evt, cts.Token);
        }

        // Sends the listen request for the user
        public async ValueTask ListenToUser(string userID, string authToken, params string[] topics)
        {
            if(readerThread == null || !readerThread.IsAlive)
            {
                cts = new CancellationTokenSource();
                readerThread = new Thread(new ParameterizedThreadStart(ReadWorker));
            }

            var startThread = false;

            if(client.State != WebSocketState.Open && client.State != WebSocketState.Connecting)
            {
                await client.ConnectAsync(Target, cts.Token);
            }

            if(userIDs.ContainsKey(userID) || pendingListenReqs.ContainsKey(userID))
            {
                // already listening to user
                return;
            }

            var req = new ListenRequest(RandomString(20), authToken, topics);
            pendingListenReqs.TryAdd(userID, req);

            if(startThread)
            {
                readerThread.Start(cts.Token);
            }

            await SendJson(req);
        }

        public async ValueTask StopListenToUser(string userID)
        {
            if(userIDs.TryRemove(userID, out var topics))
            {
                await SendJson(new UnlistenRequest(topics.ToArray()));
            } 
            else if (pendingListenReqs.TryRemove(userID, out var req))
            {
                await SendJson(new UnlistenRequest(req.Data.Topics.ToArray()));
            }            
        }

        protected T FromArraySegement<T>(ArraySegment<byte> buffer)
        {
            using (var ms = new MemoryStream(buffer.Array))
            {
                using (var ss = new StreamReader(ms, Encoding.UTF8))
                {
                    using(var jtr = new JsonTextReader(ss))
                    {
                        return JsonSerializer.CreateDefault().Deserialize<T>(jtr);
                    }
                }
            }
        }

        protected Task SendJson<T>(T data)
        {
            return client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data))), WebSocketMessageType.Text, true, cts.Token);
        }

        private static Random random = new Random();
        protected static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
