using Broker.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Broker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicationsController : ControllerBase
    {
        private readonly IBrokerService brokerService;

        public PublicationsController(IBrokerService brokerService)
        {
            this.brokerService = brokerService;
        }

        [HttpPost]
        public async Task Post([FromBody]Publication publication)
        {
            await brokerService.ProcessPublication(publication);
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Hello");
        }
    }
}
