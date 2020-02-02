using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.Configuration
{
    public class PluginDetails
    {
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }
        public string Version { get; set; } = "1.0.0.0";
        public string Culture { get; set; } = "neutral";
        public string PublicKeyToken { get; set; } = "null";

        public string PluginName { get; set; }

        public override string ToString()
        {
            return $"{TypeName}, {AssemblyName}, Version={Version}, Culture={Culture}, PublicKeyToken={PublicKeyToken}";
        }
    }
}
