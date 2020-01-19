using System;
using System.Collections.Generic;
using System.Text;
using ModEventBridge.Plugin.Plugin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using ModEventBridge.TwitchPubsubPlugin.Pubsub;
using System.Threading.Channels;
using ModEventBridge.Plugin.EventSource;
using System.Threading.Tasks;

namespace ModEventBridge.TwitchPubsubPlugin.Plugin
{
    public class TwitchPubsubPlugin : IPlugin
    {
        Configuration.Configuration config;
        PubsubClient client;

        public ChannelReader<Event> Reader => client?.Events;

        public IReadOnlyList<string> UserIDs => throw new NotImplementedException();

        public void Initialize(string pluginPath)
        {
            config = new ConfigurationBuilder().
                AddJsonFile(ConfigurationPath.Combine(pluginPath, "config.json")).
                Build().
                Get<Configuration.Configuration>();

            client = new PubsubClient(
                new Uri(config.PubsubUri), 
                new BoundedChannelOptions(config.ChannelBufferSize)
                {
                    SingleWriter = true,
                    SingleReader = true
                });
            
        }

        public ValueTask DeregisterUser(string userID) => client.StopListenToUser(userID);

        public ValueTask RegisterUser(string userID)
        {
            string auth;
            if(!config.UserAuthorizations.TryGetValue(userID, out auth))
            {
                throw new Exception($"Unable to locate auth for user: {userID}");
            }

            return client.ListenToUser(userID, auth, config.TopicTemplates.ConvertAll(t => string.Format(t, userID)).ToArray());
        }
    }
}
