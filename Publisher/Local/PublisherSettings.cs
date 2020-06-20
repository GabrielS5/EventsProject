
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace Publisher.Local
{
    public class PublisherSettings
    {
        public string Name { get; set; }
        public string InputPath { get; set; }
        public List<string> BrokersUrls { get; set; }
        public int NumberOfInstances { get; set; }
        public string StatisticsUrl { get; set; }
        public int WaitTimeInMs { get; set; }

        public static PublisherSettings ReadSettings()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            var settings = new PublisherSettings();
            configuration.GetSection("Settings").Bind(settings);

            return settings;
        }
    }
}
