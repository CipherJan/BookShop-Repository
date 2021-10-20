﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookShop.Core.Models;
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
            return await _shopService.OrederBooksToShop(shopId, numberOfBooks);
        }

        [HttpPost("/update-book")]
        public async Task<Result> UpdateBook([FromBody] BookModel model)
        {
            return await _shopService.UpdateBook(model);
        }

        [HttpPost("/buy-book/{bookId}/{shopId}")]
        public async Task<Result> BuyBook(Guid bookId, int shopId)
        {
            return await _shopService.BuyBookFromShop(bookId, shopId);
        }

        [HttpGet("/get-book/{bookId}/{shopId}")]
        public async Task<BookModelOutput> GetBook(Guid bookId, int shopId)
        {
            return await _shopService.GetBookFromShop(bookId, shopId);
        }

        [HttpGet("/get-all-books/{shopId}")]
        public async Task<IEnumerable<BookModelOutput>> GetAllBooks(int shopId)
        {
            return await _shopService.GetAllBooksFromShop(shopId);
        }

        [HttpPost("/add-shop")]
        public async Task<Result> AddShop([FromBody] ShopModel model)
        {
            return await _shopService.AddShop(model);
        }
        [HttpGet("/get-shop/{shopId}")]
        public async Task<ShopModelOutput> GetShop(int shopId)
        {
            return await _shopService.GetShop(shopId);
        }

        [HttpGet("/get-all-shops")]
        public async Task<IEnumerable<ShopModelOutput>> GetAllShops()
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
         * 1. унести IDataService в другой проект
         * 2. добавить solution с контрактами
         * 3. создать контракты для запроса книг и отправки книг
         * 4. добавить в этот проект всё необходимое для работы с RMQ
         * 5. добавить отправку сообщения для запроса книг
         * 6. во второй проект добавить консюмера для приёма и обработки сообщения о запросе книг
         * 7. добавить во второй проект продюсера для отправки данных по книгам
         * 8. сюда добавиь консюмера для приёма сообщения с данными о книгах
         *
         * ......
         *
         * N. завернуть приложения в докер 
         */


        /*
         * 1. сделать хороший readme с описанием проекта, в чем его смысл, как запускать и тд
         */
    }
}