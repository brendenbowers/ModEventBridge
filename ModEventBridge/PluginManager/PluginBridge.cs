using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModEventBridge.Plugin.Plugin;

namespace ModEventBridge.PluginManager
{
    public class PluginBridge : IUserEventPlugin
    {
        public List<IEventPlugin> EventPlugins { get; set; }
        public List<IOutputPlugin> OutputPlugins { get; set; }

        protected List<Thread> pluginThreads = new List<Thread>();

        protected CancellationTokenSource cts;

        public PluginBridge() { }

        public PluginBridge(List<IEventPlugin> eventPlugins, List<IOutputPlugin> outputPlugins) : this()
        {
            EventPlugins = eventPlugins;
            OutputPlugins = outputPlugins;
        }

        public void Start()
        {
            cts = new CancellationTokenSource();
            foreach(var p in EventPlugins)
            {
                var t = new Thread(new ParameterizedThreadStart(runner));
                pluginThreads.Add(t);
                t.Start((cts.Token, p));
            }
        }

        public void Stop()
        {
            cts.Cancel();
        }

        protected async void runner(object d)
        {
            var (ct, plugin) = ((CancellationToken ct,IEventPlugin plugin))d;
            while(!ct.IsCancellationRequested)
            {
                try
                {
                    while (await plugin.Reader.WaitToReadAsync(ct))
                    {
                        while (plugin.Reader.TryRead(out var evt))
                        {
                            try
                            {
                                await Task.WhenAll(OutputPlugins.ConvertAll((op) => op.Writer.WriteAsync(evt, ct).AsTask()));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Exception writing to plugin: {ex}");
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {

                }
            }
        }


        public ValueTask RegisterUser(string userID)
        {
            return new ValueTask(Task.WhenAll(EventPlugins.ConvertAll((p) => p.RegisterUser(userID).AsTask()).ToArray()));
        }

        public ValueTask DeregisterUser(string userID)
        {
            return new ValueTask(Task.WhenAll(EventPlugins.ConvertAll((p) => p.DeregisterUser(userID).AsTask()).ToArray()));
        }
    }
}
