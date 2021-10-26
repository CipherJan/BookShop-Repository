using System.Collections.Generic;
using System.Threading.Tasks;
using BookProvider.Core;
using BookProvider.Core.ExternalAPI;
using BookProvider.Infrastructure.BookService.Interface;
using BookProvider.Infrastructure.ProxyService.Interface;

namespace BookProvider.Infrastructure.BookService
{
    public class BookService : IBookService
    {
        private const int DefaultNumberOfBooks = 1;

        private readonly IDataService _dataService;
        private readonly ExternalAPIConfiguration _externalAPIConfiguration;

        public BookService(IDataService dataService, ExternalAPIConfiguration externalAPIConfiguration)
        {
            _dataService = dataService;
            _externalAPIConfiguration = externalAPIConfiguration;
        }
        public async Task<IEnumerable<Book>> GetBooks(int numberOfBooks)
        {
            return await _dataService.GetData<List<Book>>(_externalAPIConfiguration.GetUrlAddress(SetNumberOfBooks(numberOfBooks)));
        }
        private int SetNumberOfBooks (int numberOfBooks) => numberOfBooks >= 1 ? numberOfBooks : DefaultNumberOfBooks;
    }
}
