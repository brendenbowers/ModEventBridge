using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.Bits
{
    public class Bits
    {
        [Newtonsoft.Json.JsonProperty("bits_used")]
        public int BitsUsed { get; set; }
        [Newtonsoft.Json.JsonProperty("channel_id")]
        public string ChannelID { get; set; }
        [Newtonsoft.Json.JsonProperty("chat_message")]
        public string ChatMessage { get; set; }
        [Newtonsoft.Json.JsonProperty("context")]
        public string Context { get; set; }
        [Newtonsoft.Json.JsonProperty("is_anonymous")]
        public bool IsAnonymous { get; set; }
        [Newtonsoft.Json.JsonProperty("message_id")]
        public string MessageID { get; set; }
        [Newtonsoft.Json.JsonProperty("message_type")]
        public string MessageType { get; set; }
        [Newtonsoft.Json.JsonProperty("time")]
        public DateTime Time { get; set; }
        [Newtonsoft.Json.JsonProperty("total_bits_used")]
        public int TotalBitsUsed { get; set; }
        [Newtonsoft.Json.JsonProperty("user_id")]
        public string UserID { get; set; }
        [Newtonsoft.Json.JsonProperty("user_name")]
        public string UserName { get; set; }
        [Newtonsoft.Json.JsonProperty("version")]
        public string Version { get; set; }
    }
}
