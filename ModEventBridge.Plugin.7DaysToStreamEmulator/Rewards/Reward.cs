using ModEventBridge.Plugin.EventSource;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModEventBridge.Plugin._7DaysToSTreamEmulator.Rewards
{
    public class Reward
    {
        public Event Event { get; set; }

        public Reward(Event evt)
        {
            Event = evt;
        }
    }
}
