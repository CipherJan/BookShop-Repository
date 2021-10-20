using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using BookProvider.Infrastructure.BookService;

namespace BookProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet, Route("/get-books/{numberOfBooks}")]
        public async Task<IEnumerable<Book>> GetBooks(int numberOfBooks)
        {
            return await _bookService.GetBooks(numberOfBooks);
        }
    }
}
