using MassTransit;
using System.Threading.Tasks;
using JetBrains.Annotations;
using BookContractLibrary;
using BookShop.Services.Interfaces.Services;

namespace BookShop.Consumers
{
    [UsedImplicitly]
    public class ResponseConsumer : IConsumer<IResponseContract>
    {
        private readonly IShopService _shopService;

        public ResponseConsumer(IShopService shopService)
        {
            _shopService = shopService;
        }
        public async Task Consume(ConsumeContext<IResponseContract> context)
        {
            await _shopService.AcceptBooksDelivery(context.Message);
        }
    }
}
