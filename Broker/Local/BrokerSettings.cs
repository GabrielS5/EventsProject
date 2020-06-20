using System.Collections.Generic;

namespace Broker.Local
{
    public class BrokerSettings
    {
        public string Name { get; set; }
        public List<string> Items { get; set; }
        public List<string> Peers { get; set; }
        public string StatisticsUrl { get; set; }
    }
}
