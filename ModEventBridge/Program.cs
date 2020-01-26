using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using ModEventBridge.Plugin.Plugin;

namespace ModEventBridge
{
    class Program
    {
       static Configuration.AppConfiguration config;

        static async Task Main(string[] args)
        {
            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"));
            var configs = new ConfigurationBuilder().
                AddJsonFile(path)
                .Build();
            var dbg = configs.GetDebugView();
            config = configs.Get<Configuration.AppConfiguration>();

            var manager = new PluginManager.PluginLoader(config.PluginPath);
            /*var loaded = manager.GetPlugins<IEventPlugin>();
            foreach(var lp in loaded)
            {
                await lp.Plugin.Initialize(lp.Path);
            }*/

            var eventPlugins = manager.GetPluginsForTypes<IEventPlugin>(config.EventPlugins.ToArray());
            foreach (var lp in eventPlugins)
            {
                await lp.Plugin.Initialize(lp.Path);
            }

            var outputPlugins = manager.GetPluginsForTypes<IOutputPlugin>(config.OutputPlugins.ToArray());
            var bridge = new PluginManager.PluginBridge(eventPlugins.ConvertAll(p => p.Plugin), outputPlugins.ConvertAll(p => p.Plugin));
            foreach (var lp in outputPlugins)
            {
                await lp.Plugin.Initialize(lp.Path, bridge);
            }

            bridge.Start();
            Console.ReadLine();
            bridge.Stop();
            foreach(var d in outputPlugins.FindAll(c => c is IAsyncDisposable).ConvertAll(c => c as IAsyncDisposable))
            {
                await d.DisposeAsync();
            }
        }
    }
}
