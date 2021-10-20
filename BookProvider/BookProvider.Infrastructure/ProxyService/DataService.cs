using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Net;
using Newtonsoft.Json;

namespace BookProvider.Infrastructure.ProxyService
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private const int NUMBER_OF_BOOKS = 5;
        private readonly string _defaultUrl = @"https://localhost:5005/api/v1/books?numberOfBooks=" + $"{NUMBER_OF_BOOKS}";

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetData<T>([CanBeNull] string url) where T : class
        {
            var response = await GetJsonResponseAsync<T>(url ?? _defaultUrl, HttpMethod.Get);
            return response;
        }

        private async Task<T> GetJsonResponseAsync<T>(string url, HttpMethod method, string content = null) where T : class
        {
#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback = (a, s, d, f) => true;
#endif
            try
            {
                var httpRequest = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(url),
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
