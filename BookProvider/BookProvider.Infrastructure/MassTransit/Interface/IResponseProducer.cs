using System.Threading.Tasks;

namespace BookProvider.Producer.Interface
{
    public interface IResponseProducer
    {
        public Task SentBooksResponseEvent(int toShopId, int numberOfBooks);
    }
}
