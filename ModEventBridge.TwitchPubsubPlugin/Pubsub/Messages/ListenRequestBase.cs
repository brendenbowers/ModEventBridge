using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Messages
{
    public abstract class ListenRequestBase : Message
    {
        [Newtonsoft.Json.JsonProperty("nonce")]
        public string Nonce { get; set; }
        [Newtonsoft.Json.JsonProperty("data")]
        public ListenData Data { get; set; }

        protected ListenRequestBase(string type = "") : base(type)
        {
        }

        protected ListenRequestBase(string type, string nonce, string authToken, params string[] topics) : this(type)
        {
            Nonce = nonce;
            Data = new ListenData(authToken, topics);
        }
    }
}
