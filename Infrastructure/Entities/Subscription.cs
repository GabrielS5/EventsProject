using System;

namespace Infrastructure.Entities
{
    public class Subscription : Entity
    {
        public string Item { get; set; }
        public string Value { get; set; }
        public string Comparator { get; set; }
        public string Url { get; set; }
    }
}
