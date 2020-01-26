using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Messages
{
    public class TopicMessage : Message
    {
        [Newtonsoft.Json.JsonProperty("data")]
        public TopicMessageData Data { get; set; }

        public TopicMessage() : base() { }
    }
}
