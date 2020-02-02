using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Text;

namespace ModEventBridge.PluginManager
{
    public class LoadedPlugin<T>
    {
        public T Plugin { get; set; }
        public string Path { get; set; }
        public AssemblyLoadContext LoadContext { get; set; }
    }
}
