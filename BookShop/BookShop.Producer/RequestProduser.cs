using System.Threading.Tasks;
using BookShop.Producer.Interface;
using MassTransit;
using BookShop.Producer.Contracts;

namespace BookShop.Producer
{
    public class RequestProduser : IRequestProduser
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RequestProduser(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task SendBooksRequestEvent(int fromShopId, int numberOfBooks)
        {
            await _publishEndpoint.Publish<RequestContract>(new RequestContract { FromShopId = fromShopId, NumberOfBooks = numberOfBooks });
        }
    }
}
