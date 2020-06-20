using Broker.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Broker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IBrokerService brokerService;

        public SubscriptionsController(IBrokerService brokerService)
        {
            this.brokerService = brokerService;
        }

        [HttpPost]
        public async Task Post([FromBody]Subscription subscription)
        {
            await brokerService.ProcessSubscription(subscription);
        }
    }
}
