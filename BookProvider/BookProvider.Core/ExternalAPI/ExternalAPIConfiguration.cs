using System;

namespace BookProvider.Core.ExternalAPI
{
    public class ExternalAPIConfiguration
    {
        public string ExternalAPIAddress { get; set; }
        public Uri GetUrlAddress(int number)
        {
            return new Uri($"{ExternalAPIAddress}/api/v1/books?numberOfBooks={number}");
        }
    }
}
