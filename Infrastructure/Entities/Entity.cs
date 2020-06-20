using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class Entity
    {
        public Guid Id { get; set; }
        public List<Guid> SentBy { get; set; } = new List<Guid>();
    }
}
