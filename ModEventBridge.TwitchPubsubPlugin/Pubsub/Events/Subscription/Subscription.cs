using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.Subscription
{
    public class Subscription
    {
        [Newtonsoft.Json.JsonProperty("user_name")]
        public string UserName { get; set; }
        [Newtonsoft.Json.JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [Newtonsoft.Json.JsonProperty("channel_name")]
        public string ChannelName { get; set; }
        [Newtonsoft.Json.JsonProperty("user_id")]
        public string UserID { get; set; }
        [Newtonsoft.Json.JsonProperty("channel_id")]
        public string ChannelID { get; set; }
        [Newtonsoft.Json.JsonProperty("time")]
        public DateTime Time { get; set; }
        [Newtonsoft.Json.JsonProperty("sub_plan")]
        public string SubPlan { get; set; }
        [Newtonsoft.Json.JsonProperty("sub_plan_name")]
        public string SubPlanName { get; set; }
        [Newtonsoft.Json.JsonProperty("cumulative_months")]
        public int CumulativeMonths { get; set; }
        [Newtonsoft.Json.JsonProperty("streak_months")]
        public int? StreakMonths { get; set; }
        [Newtonsoft.Json.JsonProperty("months")]
        public int Months { get; set; }
        [Newtonsoft.Json.JsonProperty("context")]
        public string Context { get; set; }
        [Newtonsoft.Json.JsonProperty("sub_message")]
        public SubMessage SubMessage { get; set; }
        [Newtonsoft.Json.JsonProperty("recipient_id")]
        public string RecipientID { get; set; }
        [Newtonsoft.Json.JsonProperty("recipient_user_name")]
        public string RecipientUserName { get; set; }
        [Newtonsoft.Json.JsonProperty("recipient_display_name")]
        public string RecipientDisplayName { get; set; }

    }

    public class SubMessage
    {
        [Newtonsoft.Json.JsonProperty("message")]
        public string Message { get; set; }
        [Newtonsoft.Json.JsonProperty("emotes")]
        public List<Emote> Emotes { get; set; }
    }

    public class Emote
    {
        [Newtonsoft.Json.JsonProperty("start")]
        public int Start { get; set; }
        [Newtonsoft.Json.JsonProperty("end")]
        public int End { get; set; }
        [Newtonsoft.Json.JsonProperty("id")]
        public int ID { get; set; }
    }
}
