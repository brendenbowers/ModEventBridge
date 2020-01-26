using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.Configuration
{
    public class AppConfiguration
    {
        public string PluginPath { get; set; }

        public List<string> EventPlugins { get; set; }
        public List<string> OutputPlugins { get; set; }
    }
}
