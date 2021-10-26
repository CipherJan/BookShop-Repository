using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using BookProvider.Infrastructure.ProxyService.Interface;
using Newtonsoft.Json;
using System.Net;

namespace BookProvider.Infrastructure.ProxyService
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetData<T>([NotNull] Uri uri) where T : class
        {
            var response = await GetJsonResponseAsync<T>(uri, HttpMethod.Get);
            return response;
        }

        private async Task<T> GetJsonResponseAsync<T>(Uri uri, HttpMethod method, string content = null) where T : class
        {
#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback = (a, s, d, f) => true;
#endif
            try
            {
                var httpRequest = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = uri,
                    Content = new StringContent(content ?? string.Empty, Encoding.UTF8, "application/json")
                };
                var response = await _httpClient.SendAsync(httpRequest);
                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(json);
            }
            finally
            {
#if DEBUG
                ServicePointManager.ServerCertificateValidationCallback = null;
#endif
            }

        }
    }
}
