using System.Collections.Generic;
using System.Threading.Tasks;
using BookProvider.Core;

namespace BookProvider.Infrastructure.BookService.Interface
{
    public interface IBookService
    {
        public Task<List<Book>> GetBooks(int numberOfBooks);
    }
}
