using Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Broker.Services
{
    public interface ISubscriptionsService
    {
        Task Add(Subscription subscription);
        Task<List<Subscription>> Get();
    }
}
