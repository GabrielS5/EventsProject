using Infrastructure.Entities;
using Infrastructure.Tools;
using System.Collections.Concurrent;
using System.Threading;

namespace Broker.Local
{
    public static class RequestsSender
    {
        public static ConcurrentStack<RequestEnvelope<Subscription>> Subscriptions = new ConcurrentStack<RequestEnvelope<Subscription>>();
        public static ConcurrentStack<RequestEnvelope<Publication>> Publications = new ConcurrentStack<RequestEnvelope<Publication>>();

        public static void Process()
        {
            while (true)
            {
                Subscriptions.TryPop(out var subscription);
                if (subscription != null)
                {
                    var dataSender = new DataSender<Subscription>(subscription.Destination);
                    dataSender.Send(subscription.Resource);
                }

                Publications.TryPop(out var publication);
                if (publication != null)
                {
                    var dataSender = new DataSender<Publication>(publication.Destination);
                    dataSender.Send(publication.Resource);
                }

                if (publication == null && subscription == null)
                    Thread.Sleep(100);
            }
        }
    }
}
