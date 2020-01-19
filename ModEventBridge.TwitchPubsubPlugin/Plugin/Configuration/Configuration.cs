using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Plugin.Configuration
{
    public class Configuration
    {
        public string PubsubUri { get; set; }
        public Dictionary<string,string> UserAuthorizations { get; set; }
        public List<string> TopicTemplates { get; set; }
        public int ChannelBufferSize { get; set; }
    }
}
