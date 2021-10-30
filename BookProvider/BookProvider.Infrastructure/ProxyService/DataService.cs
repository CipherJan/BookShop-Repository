using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BookProvider.Core;
using JetBrains.Annotations;
using BookProvider.Infrastructure.ProxyService.Interface;
using Newtonsoft.Json;

namespace BookProvider.Infrastructure.ProxyService
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Book>> GetBooks(string hostUrl, int setNumberOfBooks)
        {
            var url =  $"{hostUrl}/api/v1/books?numberOfBooks={setNumberOfBooks}";
            var response = await GetJsonResponseAsync<IEnumerable<Book>>(url, HttpMethod.Get);
            return response;
        }

        private async Task<T> GetJsonResponseAsync<T>(string url, HttpMethod method, string content = null) where T : class
        {
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

    }
}
