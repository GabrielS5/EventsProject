using Infrastructure.Entities;
using System;

namespace Infrastructure.Tools
{
    public static class Extensions
    {
        public static ItemModel ToItemModel(this Entity entity, bool isFinished, string name)
        {
            return new ItemModel
            {
                Id = entity.Id,
                IsFinished = isFinished,
                Type = entity is Publication ? TypeEnum.Publication : TypeEnum.Subscription,
                Checkpoint = new Checkpoint
                {
                    Name = name,
                    Time = DateTime.UtcNow
                }
            };
        }
    }
}
