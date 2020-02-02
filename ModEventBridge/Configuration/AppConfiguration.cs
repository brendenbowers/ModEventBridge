using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.Configuration
{
    public class AppConfiguration
    {
        public string PluginPath { get; set; }

        public List<PluginDetails> EventPlugins { get; set; }
        public List<PluginDetails> OutputPlugins { get; set; }
    }
}
