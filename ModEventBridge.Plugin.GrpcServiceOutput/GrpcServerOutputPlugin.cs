using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ModEventBridge.Plugin.EventSource;
using ModEventBridge.Plugin.Plugin;
using static ModEventBridge.Plugin.EventSource.Service.BridgeEvents;
using Grpc.Core;
using System.Threading;

namespace ModEventBridge.Plugin.GrpcServiceOutput
{
    public class GrpcServerOutputPlugin : BridgeEventsBase, IOutputPlugin, IAsyncDisposable
    {
        Configuration.PluginConfiguration config;
        protected Channel<Event> channel;

        protected Server grpcServer;

        public GrpcServerOutputPlugin()
        {
        }

        public ChannelWriter<Event> Writer => channel?.Writer;

        public IUserEventPlugin UserEventPlugin { get; protected set; }

        public ValueTask Initialize(string path, IUserEventPlugin userEventPlugin = null)
        {
            UserEventPlugin = userEventPlugin;

            var fi = new FileInfo(Path.Combine(path, "config.json"));
            using (var fs = fi.OpenRead())
            {
                config = new ConfigurationBuilder().
                AddJsonStream(fs).
                Build().
                Get<Configuration.PluginConfiguration>();
            }

            channel = System.Threading.Channels.Channel.CreateBounded<Event>(config?.ChannelBuffer ?? 500);

            grpcServer = new Server
            {
                Services = { BindService(this) },
            };

            if((config?.Listens?.Count ?? 0) > 0)
            {
                foreach(var l in config.Listens)
                {
                    grpcServer.Ports.Add(new ServerPort(l.ListenHost, l.ListenPort, ServerCredentials.Insecure));
                }
            }
            else
            {
                grpcServer.Ports.Add(new ServerPort("localhost", 43388, ServerCredentials.Insecure));
            }

            grpcServer.Start();

            return new ValueTask();
        }

        public override async Task StreamEvents(IAsyncStreamReader<EventSource.Service.StreamRequest> requestStream, IServerStreamWriter<Event> responseStream, ServerCallContext context)
        {

            var cancelWriter = new CancellationTokenSource();
            var lts = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken, cancelWriter.Token);
            var writerTask = Task.Run( async () => 
            {
                while (!context.CancellationToken.IsCancellationRequested)
                {
                    while( await channel.Reader.WaitToReadAsync(lts.Token))
                    {
                        while(channel.Reader.TryRead(out var evt))
                        {
                            await responseStream.WriteAsync(evt);
                        }
                    }
                }
            }, lts.Token);

            while(await requestStream.MoveNext(context.CancellationToken))
            {
                if(UserEventPlugin != null)
                {
                    switch (requestStream.Current.RequestType)
                    {
                        case EventSource.Service.StreamRequestType.StartStreamEvents:
                            await UserEventPlugin.RegisterUser(requestStream.Current.UserId);
                            break;
                        case EventSource.Service.StreamRequestType.StopStreamEvents:
                            await UserEventPlugin.DeregisterUser(requestStream.Current.UserId);
                            break;
                    }
                }                
            }

            cancelWriter.Cancel();
            if(!writerTask.IsCanceled && !writerTask.IsCompleted && !writerTask.IsFaulted)
            {
                writerTask.Dispose();
            }
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(grpcServer.ShutdownAsync());
        }
    }
}
