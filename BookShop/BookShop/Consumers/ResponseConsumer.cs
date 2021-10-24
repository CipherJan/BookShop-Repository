﻿using System.Threading.Tasks;
using BookContractLibrary;
using BookShop.Services.Interfaces.Services;
using JetBrains.Annotations;
using MassTransit;

namespace BookShop.Consumers
{
    [UsedImplicitly]
    public class ResponseConsumer : IConsumer<IResponseContract<BookContract>>
    {
        private readonly IShopService _shopService;

        public ResponseConsumer(IShopService shopService)
        {
            _shopService = shopService;
        }
        public async Task Consume(ConsumeContext<IResponseContract<BookContract>> context)
        {
            await _shopService.AcceptBooksDelivery(context.Message);
        }
    }
}