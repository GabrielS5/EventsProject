using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace Subscriber.Local
{
    public class SubscriberSettings
    {
        public string Name { get; set; }
        public string InputPath { get; set; }
        public List<string> BrokersUrls { get; set; }
        public string Url { get; set; }
        public string StatisticsUrl { get; set; }
        public int WaitTimeInMs { get; set; }

        public static SubscriberSettings ReadSettings()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();
            var settings = new SubscriberSettings();
            configuration.GetSection("Settings").Bind(settings);

            return settings;
        }
    }
}