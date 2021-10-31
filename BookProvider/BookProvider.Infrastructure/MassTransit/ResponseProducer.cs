using BookContractLibrary;
using MassTransit;
using System;
using System.Linq;
using System.Threading.Tasks;
using BookProvider.Core.MassTransit;
using BookProvider.Infrastructure.BookService.Interface;
using BookProvider.Infrastructure.MassTransit.Interface;
using BookProvider.Infrastructure.MassTransit.Contract;

namespace BookProvider.Infrastructure.MassTransit
{
    public class ResponseProducer : IResponseProducer
    {
        private readonly IBookService _bookService;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly MassTransitConfiguration _massTransitConfig;
        private readonly QueueEndpoints _queueEndpoints;

        public ResponseProducer(IBookService bookService, ISendEndpointProvider sendEndpointProvider, MassTransitConfiguration massTransitConfig, QueueEndpoints queueEndpoints)
        {
            _bookService = bookService;
            _sendEndpointProvider = sendEndpointProvider;
            _massTransitConfig = massTransitConfig;
            _queueEndpoints = queueEndpoints;
        }

        public async Task SentBooksResponseEvent(int toShopId, int numberOfBooks)
        {
            var books = await _bookService.GetBooks(numberOfBooks);

            if (!books.Any())
                return;


            var booksContract = books.Select(x => new BookContract
            {
                Title = x.Title,
                Author = x.Title,
                Genre = x.Genre,
                Price = x.Price,
                ReleaseDate = x.ReleaseDate
            }).ToList();

            var totalPrice = books.Sum(x=>x.Price);

            var response = new ResponseContract
            {
                ToShopId = toShopId,
                TotalBooksPrice = totalPrice,
                Books = booksContract
            };

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"{_massTransitConfig.RabbitMqAddress}/{_queueEndpoints.RequestQueueName}"));
            await endpoint.Send(response);
        }
    }
}
