using System;
using System.Threading.Tasks;

namespace BookProvider.Infrastructure.ProxyService.Interface
{
    public interface IDataService 
    {
        public Task<T> GetData<T>(Uri uri) where T : class;
    }
}
