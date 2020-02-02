using System.Collections.Generic;
using System.Text;
using ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.Subscription;
using ModEventBridge.Plugin.EventSource;
using Google.Protobuf.WellKnownTypes;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Transforms
{
    public static class SubscriptionTransform
    {
        public static Event ToEvent(this Subscription sub, string raw = "")
        {
            var subData = new SubscriptionData
            {
                EventType = sub.Context,
                Plan = sub.SubPlan,
                PlanName = sub.SubPlanName,
                Message = sub.SubMessage.Message,
                // sub gifts seem to use months instead of cumilative months
                CumulativeMonths = sub.CumulativeMonths != 0 ? sub.CumulativeMonths : sub.Months,
                RecipientId = sub.RecipientID ?? "",
                RecipientUserName = sub.RecipientUserName ?? "",
                RecipientDisplayName = sub.RecipientDisplayName ?? "",
            };

            if(sub.StreakMonths.HasValue)
            {
                subData.StreakMonths = sub.StreakMonths.GetValueOrDefault(); 
            }

            return new Event
            {
                Payload = raw,
                OccurredAt = Timestamp.FromDateTime(sub.Time),
                Platform = "TWITCH",
                EventType = sub.Context ?? "",
                UserId = sub.UserID ?? "",
                UserName = sub.UserName ?? "",
                TargetId = sub.ChannelID ?? "",
                TargetName = sub.ChannelName ?? "",
                SubData = subData,
            };
        }
    }
}
