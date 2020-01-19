using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.ChannelPoints
{
    public class User
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string ID { get; set; }
        [Newtonsoft.Json.JsonProperty("login")]
        public string Login { get; set; }
        [Newtonsoft.Json.JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }
}
