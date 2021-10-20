using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookShop.Core.Entities;
using BookShop.Core.Models;
using BookShop.Core.Exceptions;
using BookShop.Infrastructure.EntityFramework;
using BookShop.Services.Interfaces.Services;
using Microsoft.Extensions.Logging;
using BookShop.Producer.Interface;
using System.Linq;

namespace BookShop.Services
{
    public class ShopService : IShopService
    {
        private readonly BookShopContextDbContextFactory _factory;
        private readonly ILogger<ShopService> _logger;
        private readonly IRequestProduser _requestProduser;
 

        public ShopService(BookShopContextDbContextFactory factory, ILogger<ShopService> logger, IRequestProduser requestProduser)
        {
            _factory = factory;
            _logger = logger;
            _requestProduser = requestProduser;
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

        public async Task<BookModelOutput> GetBookFromShop(Guid bookId, int shopId)
        {
            var database = _factory.GetContext();
            var saleStatus = await database.GetSaleStatusFromShop(shopId);
            var book = await database.GetBook(bookId);

            return new BookModelOutput(book, saleStatus);
        }

        public async Task<IEnumerable<BookModelOutput>> GetAllBooksFromShop(int shopId)
        {
            var database = _factory.GetContext();
            var books = await database.GetAllBooksFromShop(shopId);
            var saleStatus = await database.GetSaleStatusFromShop(shopId);

            return books.Select(b => new BookModelOutput(b, saleStatus));
        }

        public async Task<Result> UpdateBook(BookModel model)
        {
            
            try
            {
                var book = new Book(model);
                var database = _factory.GetContext();
                await database.UpdateBook(book);
                return Result.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to replace book {@Request}", model);
                return Result.Fail;
            }

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
            catch(Exception exception)
            {
                _logger.LogError(exception, "Failed to add store {@Request}", model);
                return Result.Fail;
            }
        }

        public async Task<ShopModelOutput> GetShop(int shopId)
        {
            var database = _factory.GetContext();
            var shop = await database.GetShop(shopId);
            return new ShopModelOutput(shop);
        }

        public async Task<IEnumerable<ShopModelOutput>> GetAllShops()
        {
            var database = _factory.GetContext();
            var shops = await database.GetShops();
            return shops.Select(s => new ShopModelOutput(s)).ToList();
        }

        public async Task<Result> OrederBooksToShop(int shopId, int numberOfBooks)
        {
            await _requestProduser.SendBooksRequestEvent(shopId, numberOfBooks);
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
                _logger.LogError(exception,"Failed to finish the sale");
                return Result.Fail;
            }
        }
        public async Task JobBookOrder()
        {
            var database = _factory.GetContext();
            var shops = await database.GetShops();
            foreach (var shop in shops)
            {
                if (shop.CheckYouNeedBooks())
                {
                    await _requestProduser.SendBooksRequestEvent(shop.Id, shop.GetNumberOfBooksToOrder());
                }
            }
        }
        public async Task JobMakeBooksOld()
        {
            var database = _factory.GetContext();
            await database.SetBookNovelty();
        }
        public async Task AcceptBooksFromConsumer(int shopId, double totalPrice, IEnumerable<BookModel> booksModel)
        {
            try
            {
                var database = _factory.GetContext();
                var books = booksModel.Select(x => new Book(x)).ToList();

                await database.AddSeveralBooksInShop(shopId, totalPrice, books);
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