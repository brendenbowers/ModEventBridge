using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using ModEventBridge.Plugin.Plugin;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ModEventBridge
{
    class Program
    {
        static Task Main(string[] args)
        {
            return CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
            ConfigureHostConfiguration(configHost =>
            {
                configHost.AddJsonFile("config.json");
            }).
            ConfigureAppConfiguration((context, config) =>
            {
            }).
            ConfigureServices((context, services) =>
            {
                services.AddSingleton(sp => context.Configuration.Get<Configuration.AppConfiguration>());
                services.AddTransient<PluginManager.PluginLoader>();
                services.AddTransient<PluginManager.PluginBridge>();
                services.AddHostedService<Service>();
            }).ConfigureLogging(loggingConfig => {
                loggingConfig.AddConsole();
                loggingConfig.AddDebug();
            });
    }
}
