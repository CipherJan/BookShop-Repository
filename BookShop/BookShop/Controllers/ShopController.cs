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

        [HttpPost("/add-book/{shopId}")]
        public async Task<Result> AddBook([FromBody] BookModel model, int shopId)
        {
            return await _shopService.AddBookToShop(model, shopId);
        }

        [HttpPost("/order-books/{shopId}/{numberOfBooks}")]
        public async Task<Result> OrderBooks(int shopId, int numberOfBooks)
        {
            return await _shopService.CreateBooksDeliveryRequest(shopId, numberOfBooks);
        }

        [HttpPost("/buy-book/{bookId}/{shopId}")]
        public async Task<Result> BuyBook(Guid bookId, int shopId)
        {
            return await _shopService.BuyBookFromShop(bookId, shopId);
        }

        [HttpGet("/get-book/{bookId}/{shopId}")]
        public async Task<BookModelState> GetBook(Guid bookId, int shopId)
        {
            return await _shopService.GetBookFromShop(bookId, shopId);
        }

        [HttpGet("/get-all-books/{shopId}")]
        public async Task<IEnumerable<BookModelState>> GetAllBooks(int shopId)
        {
            return await _shopService.GetAllBooksFromShop(shopId);
        }

        [HttpPost("/add-shop")]
        public async Task<Result> AddShop([FromBody] ShopModel model)
        {
            return await _shopService.AddShop(model);
        }
        [HttpGet("/get-shop/{shopId}")]
        public async Task<ShopModelState> GetShop(int shopId)
        {
            return await _shopService.GetShop(shopId);
        }

        [HttpGet("/get-all-shops")]
        public async Task<IEnumerable<ShopModelState>> GetAllShops()
        {
            return await _shopService.GetAllShops();
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


        /*
         * 1. покупку книги (покупка книги подразумевает увеличение баланса магазина и исчезновение этой книги из коллекции доступных книг)  done
         * 2. получить данные по одной книге done
         * 3. получить данные по всем книгам done
         * 4. обновление книги done
         * 5. запуск распродажи   done
         * 6. окончание распродажи  done
         */

        /*
         * 1. добавить миграцию  done
         * 2. навести порядок в Book  done
         * 3. разобраться с роутами done
         * 4. прочитать как работать с docker-compose done
         * 5. установить ELK done
         * 6. настроить логирование через Serilog done
         */

        /*
         * 1. унести IDataService в другой проект done
         * 2. добавить solution с контрактами   done 
         * 3. создать контракты для запроса книг и отправки книг done
         * 4. добавить в этот проект всё необходимое для работы с RMQ   done
         * 5. добавить отправку сообщения для запроса книг done
         * 6. во второй проект добавить консюмера для приёма и обработки сообщения о запросе книг done
         * 7. добавить во второй проект продюсера для отправки данных по книгам done
         * 8. сюда добавиь консюмера для приёма сообщения с данными о книгах done
         * 9. запустить джоб в работу done
         * ......
         *
         * N. завернуть приложения в докер 
         */


        /*
         * 1. сделать хороший readme с описанием проекта, в чем его смысл, как запускать и тд
         */
    }
}