using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.Plugin.TwitchPubsub.Pubsub.Events
{
    public class DataMessage<T>
    {
        [Newtonsoft.Json.JsonProperty("data")]
        public T Data { get; set; }
    }
}
