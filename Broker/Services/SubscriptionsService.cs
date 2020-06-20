using Infrastructure.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broker.Services
{
    public class SubscriptionsService : ISubscriptionsService
    {
        private readonly ConcurrentBag<Subscription> subscriptions = new ConcurrentBag<Subscription>();

        public async Task Add(Subscription subscription)
        {
            if (subscriptions.ToList().Any(s => s.Id == subscription.Id))
                return;

            subscriptions.Add(subscription);
        }

        public async Task<List<Subscription>> Get()
        {
            return subscriptions.ToList();
        }
    }
}
