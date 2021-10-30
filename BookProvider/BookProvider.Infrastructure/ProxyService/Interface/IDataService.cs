using System.Collections.Generic;
using System.Threading.Tasks;
using BookProvider.Core;

namespace BookProvider.Infrastructure.ProxyService.Interface
{
    public interface IDataService 
    {
        public Task<IEnumerable<Book>> GetBooks(string hostUrl, int setNumberOfBooks);
    }
}
