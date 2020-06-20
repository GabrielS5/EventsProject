using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class Item
    {
        public bool IsFinished { get; set; }
        public List<Checkpoint> Route { get; set; }
    }
}
