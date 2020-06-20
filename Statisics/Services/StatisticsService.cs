using Infrastructure.Entities;
using Statisics.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Statisics.Services
{
    public class StatisticsService
    {
        public StatisticsModel ComputeStatistics()
        {
            var stats = new StatisticsModel();

            stats.PublicationsCount = ItemsRepository.Publications.ToList().Count();
            stats.SubscriptionsCount = ItemsRepository.Subscriptions.ToList().Count();

            stats.PublicationsFinishedCount = ItemsRepository.Publications.ToList().Where(p => p.Value.IsFinished).Count();
            stats.SubscriptionsFinishedCount = ItemsRepository.Subscriptions.ToList().Where(s => s.Value.IsFinished).Count();

            try
            {
                var avg = GetAverageTotalTime(ItemsRepository.Publications.Where(p => p.Value.IsFinished).Select(x => x.Value).ToList());
                var ts = TimeSpan.FromTicks(Convert.ToInt64(avg)).TotalSeconds;
                stats.PublicationsAverageTotalTime = ts + " seconds";
                avg = GetAverageTotalTime(ItemsRepository.Subscriptions.Where(s => s.Value.IsFinished).Select(x => x.Value).ToList());
                ts = TimeSpan.FromTicks(Convert.ToInt64(avg)).TotalSeconds;
                stats.SubscriptionsAverageTotalTime = ts + " seconds";
            }
            catch (Exception ex)
            {
                stats.PublicationsAverageTotalTime = "unavailable";
                stats.SubscriptionsAverageTotalTime = "unabailable";
                Console.WriteLine("No worries m8 :)");
            }

            return stats;
        }

        private double GetAverageTotalTime(List<Item> items)
        {
            var times = items.Select(i => i.Route.LastOrDefault().Time.Ticks - i.Route.FirstOrDefault().Time.Ticks);
            var avg = times.Average();

            return avg;
        }

        private Dictionary<string, List<long>> ComputeTimestamps(List<Checkpoint> checkpoints)
        {
            var timestamps = new Dictionary<string, List<long>>();

            var cp = checkpoints.ToArray();
            for (int i = 1; i < cp.Length; i++)
            {
                var key = cp[i - 1].Name + "-" + cp[i].Name;
                var value = cp[i].Time.Ticks - cp[i - 1].Time.Ticks;
                if (timestamps.ContainsKey(key))
                    timestamps[key].Add(value);
                else
                    timestamps.Add(key, new List<long> { value });
            }

            return timestamps;
        }

        private Dictionary<string, double> GetAverages(Dictionary<string, List<long>> timestamps)
        {
            var averages = new Dictionary<string, double>();

            foreach (var entry in timestamps)
            {
                averages.Add(entry.Key, entry.Value.Average());
            }

            return averages;
        }
    }
}
