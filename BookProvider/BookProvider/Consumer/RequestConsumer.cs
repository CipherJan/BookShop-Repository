﻿using BookContractLibrary;
using MassTransit;
using System.Threading.Tasks;
using BookProvider.Infrastructure.MassTransit.Interface;

namespace BookProvider.Consumer
{
    public class RequestConsumer : IConsumer<IRequestContract>
    {
        private readonly IResponseProducer _responseProducer;

        public RequestConsumer(IResponseProducer responseProducer)
        {
            _responseProducer = responseProducer;
        }

        public async Task Consume(ConsumeContext<IRequestContract> context)
        {
            var message = context.Message;
            await _responseProducer.SentBooksResponseEvent(message.FromShopId, message.NumberOfBooks);
        }
    }
}

