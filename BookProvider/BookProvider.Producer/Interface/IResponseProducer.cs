using System.Threading.Tasks;

namespace BookProvider.Producer
{
    public interface IResponseProducer
    {
        public Task SentBooksResponseEvent(int toShopId, int numberOfBooks);
    }
}
