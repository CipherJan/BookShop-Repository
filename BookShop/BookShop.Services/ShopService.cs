using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookShop.Core.Entities;
using BookShop.Core.Exceptions;
using BookShop.Infrastructure.EntityFramework;
using BookShop.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using BookContractLibrary;
using BookShop.Core.Models.BookModel;
using BookShop.Core.Models.ShopModel;
using BookShop.Infrastructure.MassTransit.Interface;
using BookShop.Services.Contracts;

namespace BookShop.Services
{
    public class ShopService : IShopService
    {
        private readonly BookShopContextDbContextFactory _factory;
        private readonly ILogger<ShopService> _logger;
        private readonly IRequestProducer _requestProducer;

        public ShopService(BookShopContextDbContextFactory factory, ILogger<ShopService> logger,
            IRequestProducer requestProducer)
        {
            _factory = factory;
            _logger = logger;
            _requestProducer = requestProducer;
        }

        public async Task<Result> AddBookToShop(BookModel model, int shopId)
        {
            try
            {
                var book = new Book(model);
                var database = _factory.GetContext();
                await database.AddBookInShop(book, shopId);

                return Result.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Operation was failed");
                return Result.Fail;
            }
        }

        public async Task<BookModelState> GetBookFromShop(Guid bookId, int shopId)
        {
            var database = _factory.GetContext();
            var saleStatus = await database.GetSaleStatusFromShop(shopId);
            var book = await database.GetBook(bookId);

            return new BookModelState(book, saleStatus);
        }

        public async Task<IEnumerable<BookModelState>> GetAllBooksFromShop(int shopId)
        {
            var database = _factory.GetContext();
            var books = await database.GetAllBooksFromShop(shopId);
            var saleStatus = await database.GetSaleStatusFromShop(shopId);

            return books.Select(b => new BookModelState(b, saleStatus));
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
                _logger.LogError(exception, "Failed to add store {@Request}", model);
                return Result.Fail;
            }
        }

        public async Task<ShopModelState> GetShop(int shopId)
        {
            var database = _factory.GetContext();
            var shop = await database.GetShop(shopId);
            return new ShopModelState(shop);
        }

        public async Task<IEnumerable<ShopModelState>> GetAllShops()
        {
            var database = _factory.GetContext();
            var shops = await database.GetShops();
            return shops.Select(s => new ShopModelState(s)).ToList();
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

        public async Task<Result> StartSale(int shopId)
        {
            try
            {
                var database = _factory.GetContext();
                await database.SetSaleStatusFromShop(shopId, Sale.Active);
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
                await database.SetSaleStatusFromShop(shopId, Sale.Inactive);
                return Result.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to finish the sale");
                return Result.Fail;
            }
        }

        public async Task OrderBooksForAllShops()
        {
            var database = _factory.GetContext();
            var shops = await database.GetShops();
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

#warning выпиздить этот метод, определять старость книги по разнице между сейчас и (датой выпуска + какой-то период)
        public Task MakeBooksOld()
        {
            return Task.CompletedTask;
        }

        public async Task AcceptBooksDelivery(IResponseContract<BookContract> message)
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

                await database.AddSeveralBooksInShop(message.ToShopId, message.TotalBooksPrice, books);
            }
            catch (InsufficientFundsOnBalanceException exception)
            {
                _logger.LogError("There are not enough funds on the balance {@Request}", exception);
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to restock books", exception);
            }
        }
    }
}