using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Messages
{
    public class Message
    {
        [Newtonsoft.Json.JsonProperty("type")]
        public string Type { get; set; }

        public Message(string type = null)
        {
            Type = type;
        }

        public static Message Ping = new Message("PING");
    }
}
