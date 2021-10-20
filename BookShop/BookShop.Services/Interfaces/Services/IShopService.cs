using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookShop.Core.Models;

namespace BookShop.Services.Interfaces.Services
{
    public  interface IShopService
    {
        Task<Result> AddBookToShop(BookModel model, int shopId);
        Task<Result> BuyBookFromShop(Guid bookId, int shopId);
        Task<Result> StartSale(int shopId);
        Task<Result> CompleteSale(int shopId);
        Task<BookModelOutput> GetBookFromShop(Guid bookId, int shopId);
        Task<IEnumerable<BookModelOutput>> GetAllBooksFromShop(int shopId);
        Task<Result> UpdateBook(BookModel model);
        Task<Result> AddShop(ShopModel model);
        Task<ShopModelOutput> GetShop(int shopId);
        Task<IEnumerable<ShopModelOutput>> GetAllShops();
        Task<Result> OrederBooksToShop(int shopId, int numberOfBooks);
        Task JobBookOrder();
        Task JobMakeBooksOld();
        Task AcceptBooksFromConsumer(int shopId, double totalPrice, IEnumerable<BookModel> booksModel);

    }
}