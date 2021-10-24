using System;
using BookContractLibrary;
using BookProvider.Infrastructure.BookService;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;
using BookProvider.Infrastructure.BookService.Interface;
using BookProvider.Producer.Contract;
using BookProvider.Producer.Interface;

namespace BookProvider.Producer
{
    public class ResponseProducer : IResponseProducer
    {
        private readonly IBookService _bookService;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public ResponseProducer(IBookService bookService, ISendEndpointProvider sendEndpointProvider)
        {
            _bookService = bookService;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task SentBooksResponseEvent(int toShopId, int numberOfBooks)
        {
            var books = await _bookService.GetBooks(numberOfBooks);

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
            
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/BookShop.ReceiveBooksEndpoint"));
            await endpoint.Send(response);
        }
    }
}
