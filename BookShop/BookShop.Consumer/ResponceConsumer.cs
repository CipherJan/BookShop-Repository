using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using BookContractLibrary;
using BookShop.Core.Models;
using Microsoft.Extensions.Logging;
using BookShop.Services.Interfaces.Services;

namespace BookShop.Consumer
{
    public class ResponceConsumer : IConsumer<IResponseContract<BookContract>>
    {
        private readonly ILogger<ResponceConsumer> _logger;
        private readonly IShopService _shopService;

        public ResponceConsumer(ILogger<ResponceConsumer> logger, IShopService shopService)
        {
            _logger = logger;
            _shopService = shopService;
        }
        public async Task Consume(ConsumeContext<IResponseContract<BookContract>> context)
        {
            var message = context.Message;
            var booksModel = message.Books.Select(x => new BookModel()
            {
                Title = x.Title,
                Author = x.Title,
                Genre = x.Genre,
                Price = x.Price,
                ReleaseDate = x.ReleaseDate
            }).ToList();

            await _shopService.AcceptBooksFromConsumer(message.ToShopId, message.TotalBooksPrice, booksModel);

            _logger.LogInformation($"New arrival of books in quantity: {booksModel.Count}", message);
        }
    }
}
