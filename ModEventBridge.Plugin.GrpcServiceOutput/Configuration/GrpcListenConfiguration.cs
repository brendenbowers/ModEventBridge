using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.Plugin.GrpcServiceOutput.Configuration
{
    public class GrpcListenConfiguration
    {
        // The host to lisen on (localhost, 0.0.0.0, ect)
        public string ListenHost { get; set; }
        // The port to listen on
        public int ListenPort { get; set; }
    }
}
