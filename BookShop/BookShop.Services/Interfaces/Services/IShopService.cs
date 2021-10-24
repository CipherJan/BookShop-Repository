using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookContractLibrary;
using BookShop.Core.Models.BookModel;
using BookShop.Core.Models.ShopModel;

namespace BookShop.Services.Interfaces.Services
{
    public  interface IShopService
    {
        Task<Result> AddShop(ShopModel model);
        Task<IEnumerable<ShopModelState>> GetAllShops();
        Task<ShopModelState> GetShop(int shopId);
        
        Task<Result> AddBookToShop(BookModel model, int shopId);
        Task<IEnumerable<BookModelState>> GetAllBooksFromShop(int shopId);
        Task<BookModelState> GetBookFromShop(Guid bookId, int shopId);
        Task<Result> BuyBookFromShop(Guid bookId, int shopId);
        Task<Result> StartSale(int shopId);
        Task<Result> CompleteSale(int shopId);
        
        Task<Result> CreateBooksDeliveryRequest(int shopId, int numberOfBooks);
        Task AcceptBooksDelivery(IResponseContract<BookContract> shopId);
        Task OrderBooksForAllShops();
        Task MakeBooksOld();
        
    }
}