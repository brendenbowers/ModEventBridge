using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModEventBridge.Plugin.Plugin;

namespace ModEventBridge.PluginManager
{
    public class PluginBridge : IUserEventPlugin
    {
        public List<IEventPlugin> EventPlugins { get; set; }
        public List<IOutputPlugin> OutputPlugins { get; set; }

        protected List<Thread> pluginThreads = new List<Thread>();

        protected CancellationTokenSource cts;

        protected readonly PluginLoader loader;
        protected readonly ILogger logger;

        public PluginBridge() { }

        public PluginBridge(PluginLoader loader, ILogger<PluginBridge> logger) : this()
        {
            this.loader = loader;
            this.logger = logger;
        }

        public async ValueTask Start(bool loadPlugins = true)
        {
            if(loadPlugins)
            {
                EventPlugins = await loader.LoadEventPlugins();
                OutputPlugins = await loader.LoadOutputPlugins(this);
            }

            cts = new CancellationTokenSource();
            foreach(var p in EventPlugins)
            {
                var t = new Thread(new ParameterizedThreadStart(runner));
                pluginThreads.Add(t);
                t.Start((cts.Token, p));
            }
        }

        public async ValueTask Stop()
        {

            cts.Cancel();
            foreach (var d in OutputPlugins.FindAll(c => c is IAsyncDisposable).ConvertAll(c => c as IAsyncDisposable))
            {
                await d.DisposeAsync();
            }
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
                                logger.LogError(ex, "Exception writing to plugin");
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
