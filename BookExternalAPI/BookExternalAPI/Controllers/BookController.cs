using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Core;
using Infrastructure;

namespace BookExternalAPI.Controllers
{
    [Route("api/v1/books")]
    public class BookController : Controller
    {
        [HttpGet("")]
        public List<Book> GetBooks(int numberOfBooks)
        {
            return BookCreator.GetBooks(numberOfBooks: numberOfBooks);
        }
    }
}
