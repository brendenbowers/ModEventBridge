using ModEventBridge.Plugin.EventSource;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ModEventBridge.Plugin.Plugin
{
    public interface IOutputPlugin
    {
        // The channel events are written to
        ChannelWriter<Event> Writer { get; }
        // The user event plugin that can reques registering or unregisering users
        IUserEventPlugin UserEventPlugin { get; }
        // Initializes the plugin
        ValueTask Initialize(string path, IUserEventPlugin userEventPlugin = null);
        // Indicates the plugin should handle events for
    }
}
