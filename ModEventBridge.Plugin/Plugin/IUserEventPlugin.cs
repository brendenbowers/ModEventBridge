using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ModEventBridge.Plugin.Plugin
{
    public interface IUserEventPlugin
    {
        // Indicates the plugin should handle events for the user id
        ValueTask RegisterUser(string userID);
        // Indicates the plugin should no longer handle events for the user id
        ValueTask DeregisterUser(string userID);
    }
}
