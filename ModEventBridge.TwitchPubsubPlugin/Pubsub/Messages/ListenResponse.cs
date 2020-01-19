using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Messages
{
    public class ListenResponse : Message
    {
        [Newtonsoft.Json.JsonProperty("nonce")]
        public string Nonce { get; set; }
        [Newtonsoft.Json.JsonProperty("error")]
        public string Error { get; set; }

        public ListenResponse() : base()
        {
        }

    }
}
