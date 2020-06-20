using Infrastructure.Entities;
using Infrastructure.Tools;
using System;
using System.Threading;

namespace Subscriber.Local
{
    public class SubscriberThread
    {
        private readonly ResourceStore<Subscription> resourceStore;
        private readonly DataSender<ItemModel> statisticsDataSender;
        private readonly SubscriberSettings settings;
        private readonly Random random;

        public SubscriberThread(ResourceStore<Subscription> resourceStore, SubscriberSettings settings)
        {
            this.resourceStore = resourceStore;
            this.settings = settings;
            this.statisticsDataSender = new DataSender<ItemModel>(settings.StatisticsUrl);
            random = new Random();
        }

        public async void Procedure()
        {
            var resource = resourceStore.Get();
            while (resource != null)
            {
                resource.Id = Guid.NewGuid();
                resource.Url = settings.Url;
                Console.WriteLine($"Sending subscription: {resource.Id} {resource.Item}");
                await statisticsDataSender.SendAsync(resource.ToItemModel(false, settings.Name));
                var brokerUrl = settings.BrokersUrls[random.Next(0, settings.BrokersUrls.Count - 1)];
                var dataSender = new DataSender<Subscription>(brokerUrl);
                await dataSender.SendAsync(resource);
                Thread.Sleep(settings.WaitTimeInMs);
                resource = resourceStore.Get();
            }
        }
    }
}
