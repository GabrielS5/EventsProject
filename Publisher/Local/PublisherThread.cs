using Infrastructure.Entities;
using Infrastructure.Tools;
using System;
using System.Threading;

namespace Publisher.Local
{
    public class PublisherThread
    {
        private readonly ResourceStore<Publication> resourceStore;
        private readonly Random random;
        private readonly DataSender<ItemModel> statisticsDataSender;
        private readonly string name;
        private readonly PublisherSettings settings;

        public PublisherThread(ResourceStore<Publication> resourceStore, PublisherSettings settings, int order)
        {
            this.resourceStore = resourceStore;
            this.random = new Random();
            this.statisticsDataSender = new DataSender<ItemModel>(settings.StatisticsUrl);
            this.name = settings.Name + " " + order;
            this.settings = settings;
        }

        public void Procedure()
        {
            var resource = resourceStore.Get();
            while (resource != null)
            {
                resource.Id = Guid.NewGuid();
                Console.WriteLine($"Sending publication: {resource.Id} {resource.Item}");
                statisticsDataSender.Send(resource.ToItemModel(false, name));

                var brokerUrl = settings.BrokersUrls[random.Next(0, settings.BrokersUrls.Count - 1)];
                var dataSender = new DataSender<Publication>(brokerUrl);
                dataSender.Send(resource);
                resource = resourceStore.Get();
                Thread.Sleep(settings.WaitTimeInMs);
            }
        }
    }
}
