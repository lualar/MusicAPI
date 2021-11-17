using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.ApplicationInsights;

namespace MusicAPI
{
    public class Program
    {
        private TelemetryClient aiClient;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((context, config) =>
            {
                var root = config.Build();
                config.AddAzureKeyVault($"https://{root["KeyVault:Vault"]}.vault.azure.net/", root["KeyVault:ClientID"], root["KeyVault:ClientSecret"]);
            })
            .ConfigureLogging(logging =>
            {
                logging.AddApplicationInsights("81abe673-b044-497d-a21a-7675ac5bceb3");
                logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
