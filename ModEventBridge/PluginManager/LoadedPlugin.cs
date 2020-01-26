using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.PluginManager
{
    public class LoadedPlugin<T>
    {
        public T Plugin { get; set; }
        public string Path { get; set; }
        public AppDomain Domain { get; set; }
    }
}
