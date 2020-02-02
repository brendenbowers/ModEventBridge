using ModEventBridge.Plugin.Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Runtime.Loader;

namespace ModEventBridge.PluginManager
{
    public class PluginLoader
    {
        public readonly Configuration.AppConfiguration Config;
        public string PluginDir => Config.PluginPath;
        protected ILoggerFactory loggerFactory;
        protected ILogger logger;
        public PluginLoader(Configuration.AppConfiguration config, ILoggerFactory loggerFactory)
        {
            Config = config;
            this.loggerFactory = loggerFactory;
            logger = this.loggerFactory.CreateLogger<PluginLoader>();
        }

        public List<LoadedPlugin<T>> GetPlugins<T>()
            where T: class
        {

            List<LoadedPlugin<T>> plugins = new List<LoadedPlugin<T>>();
            foreach (var subdir in Directory.GetDirectories(PluginDir))
            {
                var abssubdir = Path.GetFullPath(subdir);
                List<Type> toInit = new List<Type>();
                foreach (var file in Directory.GetFiles(abssubdir, "*.dll"))
                {

                    var asym = Assembly.LoadFrom(file);
                    plugins.AddRange(GetLoadedPluginInAssembly<T>(asym));
                }
            }

            return plugins;
        }

        // Loads and initializes event plugins
        public ValueTask<List<IEventPlugin>> LoadEventPlugins() => LoadPlugins<IEventPlugin>(
            lp => lp.Plugin.Initialize(lp.Path), 
            Config.EventPlugins.ToArray());

        // Loads and initializes output plugins
        public ValueTask<List<IOutputPlugin>> LoadOutputPlugins(IUserEventPlugin userEventPlugin) => 
            LoadPlugins<IOutputPlugin>(
                lp => lp.Plugin.Initialize(lp.Path, userEventPlugin), 
                Config.OutputPlugins.ToArray());

        public async ValueTask<List<T>> LoadPlugins<T>(Func<LoadedPlugin<T>, ValueTask> init, params Configuration.PluginDetails[] types)
            where T : class
        {
            var plugins = GetPluginsForTypes<T>(types);
            var res = new List<T>(plugins.Count);
            foreach(var lp in plugins)
            {
                await init(lp);
                res.Add(lp.Plugin);
            }

            return res;
        }

        public List<LoadedPlugin<T>> GetPluginsForTypes<T>(params Configuration.PluginDetails[] types)
            where T : class
        {
            List<LoadedPlugin<T>> plugins = new List<LoadedPlugin<T>>();
            foreach(var pd in types)
            {
                var dll = FindDllForAssembly(pd.AssemblyName);
                if (string.IsNullOrWhiteSpace(dll))
                {
                    logger.LogWarning($"Could not locate dll for plugin: {pd}");
                    continue;
                }
                var resolver = new AssemblyDependencyResolver(Path.GetFullPath(dll));

                var loadContext = new AssemblyLoadContext(pd.PluginName);
                loadContext.Resolving += new Func<AssemblyLoadContext, AssemblyName, Assembly?>((AssemblyLoadContext ctx, AssemblyName an) =>
                {
                    var path = resolver.ResolveAssemblyToPath(an);
                    if(string.IsNullOrWhiteSpace(path))
                    {
                        return null;
                    }
                    return ctx.LoadFromAssemblyPath(path);
                });
                var asym = loadContext.LoadFromAssemblyPath(dll);


                var t = asym.GetType(pd.TypeName);
                if (t?.GetInterfaces().Contains(typeof(T)) ?? false && !t.IsInterface && !t.IsAbstract)
                {
                    
                    var plugin = t.GetConstructor(new Type[] { typeof(ILoggerFactory) })?.Invoke(new object[] { loggerFactory }) as T;
                    if(plugin == null)
                    {
                        plugin = t.GetConstructor(new Type[] { })?.Invoke(new object[] { }) as T;
                    }

                    if (plugin != null)
                    {
                        plugins.Add(new LoadedPlugin<T> { Path = Path.GetDirectoryName(t.Assembly.Location), Plugin = plugin, LoadContext = loadContext });
                    }
                }
            }

            return plugins;
        }

        protected Assembly FindAssembly(AssemblyName an)
        {

            var dllPath = FindDllForAssembly(an.Name);
            if(dllPath != null)
            {
                return Assembly.LoadFrom(dllPath);
            }

            return null;
        }

        protected string FindDllForAssembly(string asymName)
        {
            foreach (var subdir in Directory.GetDirectories(PluginDir))
            {
                var abssubdir = Path.GetFullPath(subdir);
                foreach (var file in Directory.GetFiles(abssubdir, "*.dll"))
                {
                    var fi = new FileInfo(file);
                    if (fi.Name.StartsWith(asymName))
                    {
                        return fi.FullName;
                    }
                }
            }
            return null;
        }

        protected List<LoadedPlugin<T>> GetLoadedPluginInAssembly<T>(Assembly asym)
            where T : class
        {
            var plugins = new List<LoadedPlugin<T>>();
            foreach (var t in asym.GetTypes())
            {
                if (t.GetInterfaces().Contains(typeof(T)) && !t.IsInterface && !t.IsAbstract)
                {
                    var plugin = t.GetConstructor(new Type[] { }).Invoke(new object[] { }) as T;
                    if (plugin != null)
                    {
                        plugins.Add(new LoadedPlugin<T> { Path = Path.GetDirectoryName(asym.CodeBase), Plugin = plugin });
                    }
                }
            }
            return plugins;
        }
    }
}
