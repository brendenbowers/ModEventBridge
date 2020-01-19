using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using ModEventBridge.Plugin.EventSource;

namespace ModEventBridge.Plugin.Plugin
{
    public interface IPlugin
    {
        // The channel events are written to
        ChannelReader<Event> Reader { get; }
        // The user ids that are generating events
        IReadOnlyList<string> UserIDs { get; }

        // Initializes the plugin
        void Initialize(string path);
        // Indicates the plugin should handle events for the user id
        ValueTask RegisterUser(string userID);
        // Indicates the plugin should no longer handle events for the user id
        ValueTask DeregisterUser(string userID);
    }
}
