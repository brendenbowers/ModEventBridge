using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using ModEventBridge.Plugin.EventSource;

namespace ModEventBridge.Plugin.Plugin
{
    public interface IEventPlugin : IUserEventPlugin
    {
        // The channel events are read from
        ChannelReader<Event> Reader { get; }
        // The user ids that are generating events
        IReadOnlyList<string> UserIDs { get; }

        // Initializes the plugin
        ValueTask Initialize(string path);
    }
}
