using System;
using BookShop.Core.Entities;

namespace BookShop.Core.Models.BookModel
{
    public class BookModelState
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Guid BookId { get; set; }

        public BookModelState(Book book, Sale saleStatus)
        {
            Title = book.Title;
            Author = book.Author;
            Genre = book.Genre.ToString();
            Price = GetPrice(book, saleStatus);
            ReleaseDate = book.ReleaseDate;
            BookId = book.Id;
        }

        private static double GetPrice(Book book, Sale saleStatus) => saleStatus.Equals(Sale.Active) && book.Novelty.Equals(BookNovelty.Old) ? book.DiscountPrice : book.Price;
    }
}
