namespace Infrastructure.Entities
{
    public class StatisticsModel
    {
        public int PublicationsCount { get; set; }
        public int SubscriptionsCount { get; set; }

        public int PublicationsFinishedCount { get; set; }
        public int SubscriptionsFinishedCount { get; set; }

        public double PublicationsAverageTotalTime { get; set; }
        public double SubscriptionsAverageTotalTime { get; set; }

        //public Dictionary<string, double> PublicationTimes{get;set;}

        //public Dictionary<string, double> SubscriptionsTimes { get; set; }
    }
}
