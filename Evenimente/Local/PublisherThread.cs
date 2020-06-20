using Infrastructure.Entities;
using Infrastructure.Tools;

namespace Publisher.Local
{
    public class PublisherThread
    {
        private readonly ResourceStore<Publication> resourceStore;
        private readonly DataSender<Publication> dataSender;

        public PublisherThread(ResourceStore<Publication> resourceStore, PublisherSettings settings)
        {
            this.resourceStore = resourceStore;
            this.dataSender = new DataSender<Publication>(settings.BrokersUrl);
        }

        public async void Procedure()
        {
            var resource = resourceStore.Get();
            while (resource != null)
            {
                await dataSender.Send(resource);
                resource = resourceStore.Get();
            }
        }
    }
}
