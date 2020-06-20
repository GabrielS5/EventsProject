using Infrastructure.Entities;
using Infrastructure.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Subscriber.Local;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicationsController : ControllerBase
    {
        private static StringBuilder stringBuilder = new StringBuilder();
        private readonly ILogger<PublicationsController> logger;
        private readonly DataSender<ItemModel> statisticsDataSender;
        private readonly SubscriberSettings settings;

        public PublicationsController(ILogger<PublicationsController> logger, IOptions<SubscriberSettings> settings)
        {
            this.logger = logger;
            this.settings = settings.Value;
            this.statisticsDataSender = new DataSender<ItemModel>(this.settings.StatisticsUrl);
        }

        [HttpPost]
        public async Task Post([FromBody]Publication publication)
        {
            logger.LogInformation($"Received publication: {publication.Id} {publication.Item}");
            await statisticsDataSender.SendAsync(publication.ToItemModel(true, settings.Name));
            stringBuilder.Append(publication.Value).Append("   ");
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(stringBuilder.ToString());
        }
    }
}
