using BookProvider.Infrastructure.ProxyService;
using Core;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookProvider.Infrastructure.BookService
{
    public class BookService : IBookService
    {
        private readonly IDataService _dataService;
        private readonly IConfiguration _configuration;

        private const int NUMBER_OF_BOOKS = 5;
        private readonly string _defaultUrl = @"https://localhost:5005/api/v1/books?numberOfBooks=";

        public BookService(IDataService dataService, IConfiguration configuration)
        {
            _dataService = dataService;
            _configuration = configuration;
        }

        public async Task<List<Book>> GetBooks(int numberOfBooks)
        {
            return await _dataService.GetData<List<Book>>(url:GetUrl(numberOfBooks));
        }

        private string GetUrl(int numberOfBooks)
        {
            string url = _configuration.GetConnectionString("ConnectionUrl") ?? _defaultUrl;

            if (numberOfBooks < 1)
            {
                numberOfBooks = NUMBER_OF_BOOKS;
            }
            url += numberOfBooks;

            return url;
        }

    }
}
