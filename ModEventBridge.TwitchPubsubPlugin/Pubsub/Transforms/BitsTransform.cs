using System.Collections.Generic;
using System.Text;
using ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.Bits;
using ModEventBridge.Plugin.EventSource;
using Google.Protobuf.WellKnownTypes;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Transforms
{
    public static class BitsTransform
    {
        public static Event ToEvent(this Bits bits, string raw = "")
        {
            return new Event {
               Payload = raw,
               OccurredAt = Timestamp.FromDateTime(bits.Time),
               Platform = "TWITCH",
               EventType = "bits",
               UserId = bits.UserID,
               UserName = bits.UserName,
               TargetId = bits.ChannelID,
               DonatonData = new DonationData
               {
                   Amount = bits.BitsUsed,
                   DonationType = "bits",
                   Message = bits.ChatMessage,
               }
            };
        }
    }
}
