using BookContractLibrary;
using BookProvider.Infrastructure.BookService;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace BookProvider.Producer
{
    public class ResponseProducer : IResponseProducer
    {
        private readonly IBookService _bookService;
        private readonly IPublishEndpoint _publishEndpoint;

        public ResponseProducer(IBookService bookService, IPublishEndpoint publishEndpoint)
        {
            _bookService = bookService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task SentBooksResponseEvent(int toShopId, int numberOfBooks)
        {
            var books = await _bookService.GetBooks(numberOfBooks);

            var booksContract = books.Select(x => new BookContract()
            {
                Title = x.Title,
                Author = x.Title,
                Genre = x.Genre,
                Price = x.Price,
                ReleaseDate = x.ReleaseDate
            }).ToList();

            var totalPrice = books.Sum(x=>x.Price);

            await _publishEndpoint.Publish<ResponseContract>(new ResponseContract
            {
                ToShopId = toShopId,
                TotalBooksPrice = totalPrice,
                Books = booksContract
            });
        }
    }
}
