using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BookShop.Core.Entities;
using BookShop.Core.Exceptions;
using BookShop.Core.Models.BookModel;
using BookShop.Core.Models.ShopModel;
using BookShop.Infrastructure.EntityFramework;
using BookShop.Infrastructure.MassTransit.Interface;
using BookShop.Services.Contracts;
using BookShop.Services.Interfaces.Services;
using BookContractLibrary;

namespace BookShop.Services
{
    public class ShopService : IShopService
    {
        private const double BookAcceptanceFeeInPercent = 0.07;

        private readonly BookShopContextDbContextFactory _factory;
        private readonly ILogger<ShopService> _logger;
        private readonly IRequestProducer _requestProducer;

        private static double GetTotalPrice(double price) => BookAcceptanceFeeInPercent * price;

        public ShopService(BookShopContextDbContextFactory factory, ILogger<ShopService> logger,
            IRequestProducer requestProducer)
        {
            _factory = factory;
            _logger = logger;
            _requestProducer = requestProducer;
        }

        public async Task<Result> AddShop(ShopModel model)
        {
            var shop = new Shop(model);
            try
            {
                var database = _factory.GetContext();
                await database.AddShop(shop);
                return Result.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to add store {@Model}", model);
                return Result.Fail;
            }
        }

        public async Task<IEnumerable<ShopModelState>> GetAllShops()
        {
            var database = _factory.GetContext();
            var shops = await database.GetAllShops();
            return shops.Select(s => new ShopModelState(s)).ToList();
        }

        public async Task<ShopModelState> GetShop(int shopId)
        {
            var database = _factory.GetContext();
            var shop = await database.GetShop(shopId);
            return new ShopModelState(shop);
        }

        public async Task<Result> AddBookToShop(BookModel model, int shopId)
        {
            try
            {
                var book = new Book(model);
                var database = _factory.GetContext();
                await database.AddBookToShop(book, shopId);

                return Result.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Operation was failed");
                return Result.Fail;
            }
        }

        public async Task<IEnumerable<BookModelState>> GetAllBooksFromShop(int shopId)
        {
            var database = _factory.GetContext();
            var books = await database.GetAllBooksFromShop(shopId);
            var saleStatus = await database.GetSaleStatusFromShop(shopId);

            return books.Select(b => new BookModelState(b, saleStatus));
        }

        public async Task<BookModelState> GetBookFromShop(Guid bookId, int shopId)
        {
            var database = _factory.GetContext();
            var saleStatus = await database.GetSaleStatusFromShop(shopId);
            var book = await database.GetBook(bookId);

            return new BookModelState(book, saleStatus);
        }

        public async Task<Result> BuyBookFromShop(Guid bookId, int shopId)
        {
            try
            {
                var database = _factory.GetContext();
                await database.BuyBookFromShop(bookId, shopId);
                return Result.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Unable to purchase book with Id: {bookId} from shop with Id {shopId}");
                return Result.Fail;
            }
        }

        public async Task<Result> PutMoneyToShop(int shopId, double sum)
        {
            try
            {
                var database = _factory.GetContext();
                await database.PutMoneyToShop(shopId, sum);
                return Result.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Failed to top up the balance of the store with Id {shopId}");
                return Result.Fail;
            }
        }

        public async Task<Result> StartSale(int shopId)
        {
            try
            {
                var database = _factory.GetContext();
                await database.SetSaleStatusFromShop(shopId, ShopSale.Active);
                return Result.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to start sale");
                return Result.Fail;
            }
        }

        public async Task<Result> CompleteSale(int shopId)
        {
            try
            {
                var database = _factory.GetContext();
                await database.SetSaleStatusFromShop(shopId, ShopSale.Inactive);
                return Result.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to finish the sale");
                return Result.Fail;
            }
        }

        public async Task<Result> CreateBooksDeliveryRequest(int shopId, int numberOfBooks)
        {
            await _requestProducer.SendBooksRequestEvent(new RequestContract
            {
                FromShopId = shopId,
                NumberOfBooks = numberOfBooks
            });
            return Result.Success;
        }

        public async Task OrderBooksForAllShops()
        {
            var database = _factory.GetContext();
            var shops = await database.GetAllShops();
            foreach (var shop in shops)
            {
                if (shop.CheckYouNeedBooks())
                {
                    await _requestProducer.SendBooksRequestEvent(new RequestContract
                    {
                        FromShopId = shop.Id,
                        NumberOfBooks = shop.GetNumberOfBooksToOrder()
                    });
                }
            }
        }

        public async Task AcceptBooksDelivery(IResponseContract message)
        {
            var booksModel = message.Books.Select(x => new BookModel
            {
                Title = x.Title,
                Author = x.Title,
                Genre = x.Genre,
                Price = x.Price,
                ReleaseDate = x.ReleaseDate
            }).ToList();
            try
            {
                var database = _factory.GetContext();
                var books = booksModel.Select(x => new Book(x)).ToList();

                await database.AddSeveralBooksToShop(message.ToShopId, GetTotalPrice(message.TotalBooksPrice), books);
            }
            catch (InsufficientFundsOnBalanceException exception)
            {
                _logger.LogError(exception, "There are not enough funds on the balance");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to restock books");
            }
        }
    }
}