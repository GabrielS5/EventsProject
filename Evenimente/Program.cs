using Infrastructure.Entities;
using Infrastructure.Tools;
using Publisher.Local;
using System.Collections.Generic;
using System.Threading;

namespace Publisher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var settings = PublisherSettings.ReadSettings();
            var items = Reader.Read<Publication>(settings.InputPath);
            var resourceStore = new ResourceStore<Publication>(items);
            var threads = new List<Thread>();

            for (int i = 0; i < settings.NumberOfInstances; i++)
            {
                var publisherThread = new PublisherThread(resourceStore, settings);
                var thread = new Thread(new ThreadStart(publisherThread.Procedure));
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}
