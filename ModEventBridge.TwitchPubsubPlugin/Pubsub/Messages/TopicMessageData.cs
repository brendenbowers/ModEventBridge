using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Messages
{
    public class TopicMessageData
    {
        [Newtonsoft.Json.JsonProperty("topic")]
        public string Topic { get; set; }
        [Newtonsoft.Json.JsonProperty("message")]
        public string Message { get; set; }
    }
}
