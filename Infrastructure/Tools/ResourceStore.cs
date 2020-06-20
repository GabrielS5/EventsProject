using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Infrastructure.Tools
{
    public class ResourceStore<T>
    {
        private readonly ConcurrentStack<T> resources;

        public ResourceStore(List<T> resources)
        {
            this.resources = new ConcurrentStack<T>(resources);
        }

        public T Get()
        {
            resources.TryPop(out var resource);

            return resource;
        }
    }
}
