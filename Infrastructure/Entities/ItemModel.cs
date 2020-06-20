using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class ItemModel
    {
        public Guid Id { get; set; }
        public TypeEnum Type { get; set; }

        public bool IsFinished { get; set; }

        public Checkpoint Checkpoint { get; set; }

        public Item GetAsItem()
        {
            return new Item
            {
                IsFinished = IsFinished,
                Route = new List<Checkpoint> { Checkpoint }
            };
        }
    }
}
