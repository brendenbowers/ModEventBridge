using System;
using System.Collections.Generic;
using System.Text;
using ModEventBridge.TwitchPubsubPlugin.Pubsub.Events.ChannelPoints;
using ModEventBridge.Plugin.EventSource;
using Google.Protobuf.WellKnownTypes;

namespace ModEventBridge.TwitchPubsubPlugin.Pubsub.Transforms
{
    public static class PointsTransform
    {
        public static Event ToEvent(this Points points, string raw = "")
        {
            if(string.IsNullOrWhiteSpace(raw))
            {
                raw = Newtonsoft.Json.JsonConvert.SerializeObject(points.Data);
            } 

           return new Event
            {
                Payload = raw,
                OccurredAt = Timestamp.FromDateTime(points.Data.RedeemedAt),
                Platform = "TWITCH",
                EventType = "channel_points",
                UserId = points.Data.User.ID,
                UserName = points.Data.User.Login,
                TargetId = points.Data.ChannelID,
                PointsData = new PointsData
                {
                    RewardId = points.Data.Reward.ID,
                    RewardTitle = points.Data.Reward.Title,
                    Message = points.Data.UserInput,
                    Cost = points.Data.Reward.Cost.ToString(),
                }
            };
        }
    }
}
