using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BookProvider.Core;
using BookProvider.Infrastructure.ProxyService.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookProvider.Infrastructure.ProxyService
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DataService> _logger;

        public DataService(HttpClient httpClient, ILogger<DataService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Book>> GetBooks(string hostUrl, int setNumberOfBooks)
        {
            var url =  $"{hostUrl}/api/v1/books?numberOfBooks={setNumberOfBooks}";
            try
            {
                return await GetJsonResponseAsync<IEnumerable<Book>>(url, HttpMethod.Get);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Data source unavailable");
                return new List<Book>();
            }

        }

        private async Task<T> GetJsonResponseAsync<T>(string url, HttpMethod method, string content = null) where T : class
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
    }
}
