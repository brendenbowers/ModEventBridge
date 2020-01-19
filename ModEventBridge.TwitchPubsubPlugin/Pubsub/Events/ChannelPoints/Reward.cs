using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.ChannelPoints
{
    public class Reward
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string ID { get; set; }
        [Newtonsoft.Json.JsonProperty("channel_id")]
        public string ChannelID { get; set; }
        [Newtonsoft.Json.JsonProperty("title")]
        public string Title { get; set; }
        [Newtonsoft.Json.JsonProperty("prompt")]
        public string Prompt { get; set; }
        [Newtonsoft.Json.JsonProperty("cost")]
        public int Cost { get; set; }
        [Newtonsoft.Json.JsonProperty("is_user_input_required")]
        public bool IsUserInputRequired { get; set; }
        [Newtonsoft.Json.JsonProperty("is_sub_only")]
        public bool IsSubOnly { get; set; }
        [Newtonsoft.Json.JsonProperty("is_enabled")]
        public bool IsEnabled { get; set; }
        [Newtonsoft.Json.JsonProperty("is_paused")]
        public bool IsPaused { get; set; }
        [Newtonsoft.Json.JsonProperty("is_in_stock")]
        public bool IsInStock { get; set; }
        [Newtonsoft.Json.JsonProperty("max_per_stream")]
        public MaxPerStream MaxPerStream { get; set; }
        [Newtonsoft.Json.JsonProperty("should_redemptions_skip_request_queue")]
        public bool ShouldRedemptionsSkipRequestQueue { get; set; }
    }

    public class MaxPerStream
    {
        [Newtonsoft.Json.JsonProperty("is_enabled")]
        public bool IsEnabled { get; set; }
        [Newtonsoft.Json.JsonProperty("max_per_stream")]
        public int MaxPerSteram { get; set; }

    }
}
