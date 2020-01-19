using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Messages
{
    public class UnlistenRequest : ListenRequestBase
    {
        public UnlistenRequest() : base("UNLISTEN") { }

        public UnlistenRequest(params string[] topics) : base("UNLISTEN", null, null, topics)
        {
        }
    }
}
