using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Messages
{
    public class ListenRequest : ListenRequestBase
    {
        public ListenRequest() : base("LISTEN") { }

        public ListenRequest(string nonce, string authToken, params string[] topics) : base("LISTEN", nonce, authToken, topics)
        {
        }
    }
}
