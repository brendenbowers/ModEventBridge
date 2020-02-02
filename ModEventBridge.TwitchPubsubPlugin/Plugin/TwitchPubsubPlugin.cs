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
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ModEventBridge.Plugin.TwitchPubsub.Plugin
{
    public class TwitchPubsubPlugin : IEventPlugin
    {
        protected ILoggerFactory loggerFactory;
        protected ILogger logger;
        Configuration.PluginConfiguration config;
        PubsubClient client;

        public ChannelReader<Event> Reader => client?.Events;

        public IReadOnlyList<string> UserIDs => client == null ? new List<string>() : new List<string>(client.UserIDs);

        public TwitchPubsubPlugin(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            logger = loggerFactory.CreateLogger<TwitchPubsubPlugin>();
        }

        public async ValueTask Initialize(string pluginPath)
        {
            var fi = new FileInfo(Path.Combine(pluginPath, "config.json"));
            using (var fs = fi.OpenRead())
            {
                config = new ConfigurationBuilder().
                AddJsonStream(fs).
                Build().
                Get<Configuration.PluginConfiguration>();
            }



            client = new PubsubClient(
                new Uri(config.PubsubUri),
                new BoundedChannelOptions(config.ChannelBufferSize)
                {
                    SingleWriter = true,
                    SingleReader = true
                },
                loggerFactory.CreateLogger<PubsubClient>()
                );


            if (config.ListenToAuthedUsersOnInit)
            {
                foreach (var uid in config.UserAuthorizations.Keys)
                {
                    await RegisterUser(uid);
                }
            }
        }

        public ValueTask DeregisterUser(string userID) => client.StopListenToUser(userID);

        public ValueTask RegisterUser(string userID)
        {
            string auth;
            if (!config.UserAuthorizations.TryGetValue(userID, out auth))
            {
                throw new Exception($"Unable to locate auth for user: {userID}");
            }

            return client.ListenToUser(userID, auth, config.TopicTemplates.ConvertAll(t => string.Format(t, userID)).ToArray());
        }
    }
}
