using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ModEventBridge.Plugin.Plugin;

namespace ModEventBridge
{
    public class Service : IHostedService
    {
        protected readonly ILogger logger;
        protected readonly IHostApplicationLifetime appLifetime;
        protected readonly Configuration.AppConfiguration config;
        protected readonly PluginManager.PluginLoader loader;

        protected PluginManager.PluginBridge bridge;

        public Service(
            ILogger<Service> logger,
            IHostApplicationLifetime appLifetime,
            PluginManager.PluginBridge bridge,
            Configuration.AppConfiguration config)
        {
            this.logger = logger;
            this.appLifetime = appLifetime;
            this.config = config;
            this.bridge = bridge;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return bridge.Start().AsTask();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return bridge.Stop().AsTask();
        }
    }
}
