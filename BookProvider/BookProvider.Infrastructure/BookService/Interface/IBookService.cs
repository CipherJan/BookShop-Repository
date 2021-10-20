using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookProvider.Infrastructure.BookService
{
    public interface IBookService
    {
        public Task<List<Book>> GetBooks(int numberOfBooks);
    }
}
