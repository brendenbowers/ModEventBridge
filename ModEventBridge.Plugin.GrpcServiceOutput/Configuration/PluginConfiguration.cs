using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.Plugin.GrpcServiceOutput.Configuration
{
    public class PluginConfiguration
    {
        public int ChannelBuffer { get; set; } = 500;
        public List<GrpcListenConfiguration> Listens { get; set;  }
    }
}
