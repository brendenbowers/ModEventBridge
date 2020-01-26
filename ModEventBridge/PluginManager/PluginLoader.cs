using ModEventBridge.Plugin.Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Linq;

namespace ModEventBridge.PluginManager
{
    public class PluginLoader
    {
        public string PluginDir { get; set; }
        public PluginLoader(string pluginDir)
        {
            PluginDir = pluginDir;
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


        public List<LoadedPlugin<T>> GetPluginsForTypes<T>(params string[] types)
            where T : class
        {
            List<LoadedPlugin<T>> plugins = new List<LoadedPlugin<T>>();
            foreach(var tn in types)
            {
                var t = Type.GetType(tn, FindAssembly, null);
                if (t?.GetInterfaces().Contains(typeof(T)) ?? false && !t.IsInterface && !t.IsAbstract)
                {
                    var plugin = t.GetConstructor(new Type[] { })?.Invoke(new object[] { }) as T;
                    if (plugin != null)
                    {
                        plugins.Add(new LoadedPlugin<T> { Path = Path.GetDirectoryName(t.Assembly.Location), Plugin = plugin });
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
