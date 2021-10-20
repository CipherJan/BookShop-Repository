using System.Threading.Tasks;

namespace BookProvider.Infrastructure.ProxyService
{
    public interface IDataService 
    {
        public Task<T> GetData<T>(string url) where T : class;
    }
}
