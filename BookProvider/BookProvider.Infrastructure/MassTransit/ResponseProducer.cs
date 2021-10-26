using BookContractLibrary;
using MassTransit;
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

        public ResponseProducer(IBookService bookService, ISendEndpointProvider sendEndpointProvider, MassTransitConfiguration massTransitConfig)
        {
            _bookService = bookService;
            _sendEndpointProvider = sendEndpointProvider;
            _massTransitConfig = massTransitConfig;
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

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(_massTransitConfig.GetRequestEndpoint());
            await endpoint.Send(response);
        }
    }
}
