using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Entities;

namespace Statistics.Data
{
    public class StatisticsContext : DbContext
    {
        public StatisticsContext (DbContextOptions<StatisticsContext> options)
            : base(options)
        {
        }

        public DbSet<Infrastructure.Entities.StatisticsModel> StatisticsModel { get; set; }
    }
}
