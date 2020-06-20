using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Statisics.Data;
using Statisics.Services;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Statisics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        // GET: api/<StatisticsController>
        [HttpGet]
        public StatisticsModel Get()
        {
            return new StatisticsService().ComputeStatistics();
        }

        // POST api/<StatisticsController>
        [HttpPost]
        public void Post([FromBody] ItemModel model)
        {
            Console.WriteLine("received {0}", model.Id);
            ItemsRepository.Add(model);
        }
    }
}
