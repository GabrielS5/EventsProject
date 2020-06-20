using Infrastructure.Entities;
using System.Threading.Tasks;

namespace Broker.Services
{
    public interface IBrokerService
    {
        Task ProcessPublication(Publication publication);
        Task ProcessSubscription(Subscription subscription);
    }
}
