using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookShop.Core.Models.BookModel;
using BookShop.Core.Models.ShopModel;
using BookShop.Services;
using BookShop.Services.Interfaces.Services;

namespace BookShop.Controllers
{
    [Route("api/shop")]
    [ApiController]
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpPost("/add-shop")]
        public async Task<Result> AddShop([FromBody] ShopModel model)
        {
            return await _shopService.AddShop(model);
        }

        [HttpGet("/get-all-shops")]
        public async Task<IEnumerable<ShopModelState>> GetAllShops()
        {
            return await _shopService.GetAllShops();
        }

        [HttpGet("/get-shop/{shopId}")]
        public async Task<ShopModelState> GetShop(int shopId)
        {
            return await _shopService.GetShop(shopId);
        }

        [HttpPost("/add-book/{shopId}")]
        public async Task<Result> AddBook([FromBody] BookModel model, int shopId)
        {
            return await _shopService.AddBookToShop(model, shopId);
        }

        [HttpGet("/get-all-books/{shopId}")]
        public async Task<IEnumerable<BookModelState>> GetAllBooks(int shopId)
        {
            return await _shopService.GetAllBooksFromShop(shopId);
        }

        [HttpGet("/get-book/{bookId}/{shopId}")]
        public async Task<BookModelState> GetBook(Guid bookId, int shopId)
        {
            return await _shopService.GetBookFromShop(bookId, shopId);
        }

        [HttpPost("/buy-book/{bookId}/{shopId}")]
        public async Task<Result> BuyBook(Guid bookId, int shopId)
        {
            return await _shopService.BuyBookFromShop(bookId, shopId);
        }

        [HttpPost("/order-books/{shopId}/{numberOfBooks}")]
        public async Task<Result> OrderBooks(int shopId, int numberOfBooks)
        {
            return await _shopService.CreateBooksDeliveryRequest(shopId, numberOfBooks);
        }

        [HttpPost("/start-sale/{shopId}")]
        public async Task<Result> StartSale(int shopId)
        {
            return await _shopService.StartSale(shopId);
        }

        [HttpPost("/complete-sale/{shopId}")]
        public async Task<Result> CompleteSale(int shopId)
        {
            return await _shopService.CompleteSale(shopId);
        }
    }
}