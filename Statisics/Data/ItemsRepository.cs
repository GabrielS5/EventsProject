using Infrastructure.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Statisics.Data
{
    public static class ItemsRepository
    {
        public static ConcurrentDictionary<Guid, Item> Publications { get; set; } = new ConcurrentDictionary<Guid, Item>();
        public static ConcurrentDictionary<Guid, Item> Subscriptions { get; set; } = new ConcurrentDictionary<Guid, Item>();

        public static void Add(ItemModel model)
        {
            ConcurrentDictionary<Guid, Item> repo;

            if (model.Type == TypeEnum.Publication)
                repo = Publications;
            else
                repo = Subscriptions;


            if (repo.ContainsKey(model.Id))
            {
                repo[model.Id].IsFinished = model.IsFinished;
                repo[model.Id].Route.Add(model.Checkpoint);
            }
            else
            {
                repo.TryAdd(model.Id, model.GetAsItem());
            }
        }
    }
}
