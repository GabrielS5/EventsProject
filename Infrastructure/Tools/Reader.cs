using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Infrastructure.Tools
{
    public static class Reader
    {
        public static List<T> Read<T>(string path)
        {
            var text = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<List<T>>(text);
        }
    }
}
