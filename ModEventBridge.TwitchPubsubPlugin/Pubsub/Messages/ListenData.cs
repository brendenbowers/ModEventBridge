using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Messages
{
    public class ListenData
    {
        [Newtonsoft.Json.JsonProperty("topics")]
        public List<string> Topics { get; set; }
        [Newtonsoft.Json.JsonProperty("auth_token")]
        public string AuthToken { get; set; }

        public ListenData() { }
        public ListenData(string authToken, params string[] topics) 
        {
            AuthToken = authToken;
            Topics = new List<string>(topics);
        }
    }
}
