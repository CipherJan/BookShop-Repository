using System.Threading.Tasks;

namespace BookProvider.Infrastructure.MassTransit.Interface
{
    public interface IResponseProducer
    {
        public Task SentBooksResponseEvent(int toShopId, int numberOfBooks);
    }
}
