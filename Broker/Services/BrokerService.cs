using Broker.Local;
using Infrastructure.Entities;
using Infrastructure.Tools;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broker.Services
{
    public class BrokerService : IBrokerService
    {
        private readonly BrokerSettings settings;
        private readonly ISubscriptionsService subscriptionsService;
        private readonly ILogger<BrokerService> logger;
        private readonly DataSender<ItemModel> statisticsDataSender;
        private static readonly Guid BrokerId = Guid.NewGuid();

        public BrokerService(IOptions<BrokerSettings> settings, ISubscriptionsService subscriptionsService, ILogger<BrokerService> logger)
        {
            this.settings = settings.Value;
            this.subscriptionsService = subscriptionsService;
            this.logger = logger;
            this.statisticsDataSender = new DataSender<ItemModel>(this.settings.StatisticsUrl);
        }

        public async Task ProcessPublication(Publication publication)
        {
            if (publication.SentBy.Contains(BrokerId)) return;

            await statisticsDataSender.SendAsync(publication.ToItemModel(false, settings.Name));
            logger.LogInformation($"Received publication: {publication.Id} {publication.Item}");
            if (settings.Items.Any(i => i == publication.Item))
            {
                var subscriptions = (await subscriptionsService.Get())
                    .Where(s => s.Item == publication.Item).ToList();
                var fullfilledSubscriptions = await GetFullfilledSubscriptions(subscriptions, publication);
                var urls = fullfilledSubscriptions.Select(s => s.Url).Distinct();

                logger.LogInformation($"Sending publication: {publication.Id} {publication.Item} to subscribers: {string.Join(", ", urls)}");

                foreach (var url in urls)
                {
                    RequestsSender.Publications.Push(new RequestEnvelope<Publication>
                    {
                        Resource = publication,
                        Destination = url
                    });
                }
            }
            else if (settings.Peers != null)
            {
                publication.SentBy.Add(BrokerId);
                foreach (var peer in settings.Peers)
                {
                    logger.LogInformation($"Sending publication: {publication.Id} {publication.Item} to brokers: {string.Join(", ", settings.Peers)}");
                    RequestsSender.Publications.Push(new RequestEnvelope<Publication>
                    {
                        Resource = publication,
                        Destination = peer + "/publications"
                    });
                }
            }
        }

        public async Task ProcessSubscription(Subscription subscription)
        {
            if (subscription.SentBy.Contains(BrokerId)) return;

            logger.LogInformation($"Received subscription: {subscription.Id} {subscription.Item}");

            if (settings.Items.Any(i => i == subscription.Item))
            {
                await statisticsDataSender.SendAsync(subscription.ToItemModel(true, settings.Name));
                logger.LogInformation($"Storing subscription: {subscription.Id} {subscription.Item}");

                await subscriptionsService.Add(subscription);
            }
            else if (settings.Peers != null)
            {
                subscription.SentBy.Add(BrokerId);

                await statisticsDataSender.SendAsync(subscription.ToItemModel(false, settings.Name));
                foreach (var peer in settings.Peers)
                {
                    logger.LogInformation($"Sending subscription: {subscription.Id} {subscription.Item} to brokers: {string.Join(", ", settings.Peers)}");
                    RequestsSender.Subscriptions.Push(new RequestEnvelope<Subscription>
                    {
                        Resource = subscription,
                        Destination = peer + "/subscriptions"
                    });
                }
            }
        }

        private async Task<List<Subscription>> GetFullfilledSubscriptions(List<Subscription> subscriptions, Publication publication)
        {
            var fullfilledSubscriptions = new List<Subscription>();
            if (publication.Item == "company")
            {
                var value = publication.Value;

                foreach (var subscription in subscriptions)
                {
                    var subValue = subscription.Value;

                    switch (subscription.Comparator)
                    {
                        case Constants.Equal:
                            if (value == subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                        case Constants.NotEqual:
                            if (value != subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                    }
                }
            }
            else if (publication.Item == "date")
            {
                var split = publication.Value.Split(".");
                var value = new DateTime(int.Parse(split[2]), int.Parse(split[1]), int.Parse(split[0]));

                foreach (var subscription in subscriptions)
                {
                    var subSplit = subscription.Value.Split(".");
                    var subValue = new DateTime(int.Parse(subSplit[2]), int.Parse(subSplit[1]), int.Parse(subSplit[0]));

                    switch (subscription.Comparator)
                    {
                        case Constants.Equal:
                            if (value == subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                        case Constants.NotEqual:
                            if (value != subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                        case Constants.Greater:
                            if (value > subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                        case Constants.GreaterOrEqual:
                            if (value >= subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                        case Constants.Smaller:
                            if (value < subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                        case Constants.SmallerOrEqual:
                            if (value <= subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                    }
                }
            }
            else
            {
                var value = double.Parse(publication.Value);

                foreach (var subscription in subscriptions)
                {
                    var subValue = double.Parse(subscription.Value);

                    switch (subscription.Comparator)
                    {
                        case Constants.Equal:
                            if (value == subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                        case Constants.NotEqual:
                            if (value != subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                        case Constants.Greater:
                            if (value > subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                        case Constants.GreaterOrEqual:
                            if (value >= subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                        case Constants.Smaller:
                            if (value < subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                        case Constants.SmallerOrEqual:
                            if (value <= subValue)
                                fullfilledSubscriptions.Add(subscription);
                            break;
                    }
                }
            }

            return fullfilledSubscriptions;
        }
    }
}
