using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.ChannelPoints
{
    public class Points
    {
        [Newtonsoft.Json.JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        [Newtonsoft.Json.JsonProperty("data")]
        public Redemption Data { get; set; }
    }
}
