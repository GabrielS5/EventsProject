using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Statisics.Services;

namespace Statisics.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public StatisticsModel StatisticsModel { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            StatisticsModel = new StatisticsService().ComputeStatistics();
            _logger = logger;
        }
        public void OnGet()
        {

        }
    }
}
