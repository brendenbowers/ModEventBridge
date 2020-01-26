using ModEventBridge.Plugin.EventSource;
using ModEventBridge.Plugin.Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ModEventBridge.Plugin.ConsoleOutput
{
    public class ConsoleOutputPlugin : IOutputPlugin
    {
        protected Channel<Event> channel;

        public ChannelWriter<Event> Writer => channel.Writer;

        public IUserEventPlugin UserEventPlugin { get; set; }

        public ValueTask Initialize(string path, IUserEventPlugin evtPlugin = null) 
        {
            channel = Channel.CreateBounded<Event>(new BoundedChannelOptions(500));
            _ = Task.Run(async () => {
                while(true)
                {
                    while(await channel.Reader.WaitToReadAsync())
                    {
                        while(channel.Reader.TryRead(out var evt))
                        {

                            Console.WriteLine(Google.Protobuf.JsonFormatter.ToDiagnosticString(evt));
                        }
                    }
                }
            });
            return new ValueTask();
        }
    }
}
