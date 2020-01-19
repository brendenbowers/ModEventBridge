using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Messages
{
    public class TopicMessage : Message
    {
        [Newtonsoft.Json.JsonProperty("message")]
        public TopicMessageData Message { get; set; }

        public TopicMessage() : base() { }
    }
}
