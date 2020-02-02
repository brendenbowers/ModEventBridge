using Microsoft.Extensions.Configuration;
using ModEventBridge.Plugin.EventSource;
using ModEventBridge.Plugin.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ModEventBridge.Plugin._7DaysToSTreamEmulator
{
    public class NamedPipeEmulatorPlugin : IOutputPlugin, IAsyncDisposable
    {
        protected struct Streams 
        {
            public NamedPipeServerStream ss;
            public StreamWriter writer;
        }

        protected Configuration.PluginConfiguration config;
        protected Channel<Event> channel;
        protected List<Streams> streams;

        public ChannelWriter<Event> Writer => channel?.Writer;

        public IUserEventPlugin UserEventPlugin { get; set; }

        protected CancellationTokenSource cts;

        public ValueTask DisposeAsync()
        {
            cts?.Cancel();
            return new ValueTask();
        }

        public ValueTask Initialize(string path, IUserEventPlugin userEventPlugin = null)
        {
            var fi = new FileInfo(Path.Combine(path, "config.json"));
            using (var fs = fi.OpenRead())
            {
                config = new ConfigurationBuilder().
                AddJsonStream(fs).
                Build().
                Get<Configuration.PluginConfiguration>();
            }

            cts = new CancellationTokenSource();

            channel = Channel.CreateBounded<Event>(config.ChannelBuffer);

            _ = Task.Factory.StartNew(async () => 
            {
                while(!cts.IsCancellationRequested)
                {
                    while(await channel.Reader.WaitToReadAsync(cts.Token))
                    {
                        while(channel.Reader.TryRead(out var evt))
                        {
                            foreach(var s in streams)
                            {
                                if(s.ss.IsConnected)
                                {
                                    s.writer.WriteLineAsync()
                                }
                            }
                        }
                    }
                }
            }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            return new ValueTask();
        }



    }
}
