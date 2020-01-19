using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.ChannelPoints
{
    public class Redemption
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string ID { get; set; }
        [Newtonsoft.Json.JsonProperty("user")]
        public User User { get; set; }
        [Newtonsoft.Json.JsonProperty("channel_id")]
        public string ChannelID { get; set; }
        [Newtonsoft.Json.JsonProperty("reward")]
        public Reward Reward { get; set; }
        [Newtonsoft.Json.JsonProperty("redeemed_at")]
        public DateTime RedeemedAt { get; set; }
        [Newtonsoft.Json.JsonProperty("user_input")]
        public string UserInput { get; set; }
        [Newtonsoft.Json.JsonProperty("status")]
        public string Status { get; set; }

    }
}
