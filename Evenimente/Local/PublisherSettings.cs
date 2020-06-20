using Microsoft.Extensions.Configuration;
using System.IO;

namespace Publisher.Local
{
    public class PublisherSettings
    {
        public string InputPath { get; set; }
        public string BrokersUrl { get; set; }
        public int NumberOfInstances { get; set; }

        public static PublisherSettings ReadSettings()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();
            var settings = new PublisherSettings();
            configuration.GetSection("Settings").Bind(settings);

            return settings;
        }
    }
}
