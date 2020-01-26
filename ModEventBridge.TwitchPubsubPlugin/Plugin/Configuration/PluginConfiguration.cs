using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.Plugin.TwitchPubsub.Plugin.Configuration
{
    public class PluginConfiguration
    {
        public string PubsubUri { get; set; }
        public Dictionary<string, string> UserAuthorizations { get; set; }
        public List<string> TopicTemplates { get; set; }
        public int ChannelBufferSize { get; set; }
        public bool ListenToAuthedUsersOnInit { get; set; }
    }
}
