using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tools
{
    public class DataSender<T>
    {
        private readonly HttpClient client;

        public DataSender(string url)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri(url);
        }

        public DataSender()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }

        public async Task SendAsync(T data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = content
            };

            await client.SendAsync(request);
        }

        public async Task SendAsync(T data, string url)
        {
            client.BaseAddress = new Uri(url);

            await SendAsync(data);
        }

        public void Send(T data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = content
            };

            _ = client.SendAsync(request).Result;
        }

        public void Send(T data, string url)
        {
            client.BaseAddress = new Uri(url);

            Send(data);
        }
    }
}
