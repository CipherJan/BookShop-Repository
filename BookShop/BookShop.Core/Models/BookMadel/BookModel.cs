using BookShop.Core.Entities;
using System;

namespace BookShop.Core.Models
{
    public class BookModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }

        public BookModel()
        {
        }

        public BookModel(Book book, Sale saleStatus)
        {
            Title = book.Title;
            Author = book.Author;
            Genre = book.Genre.ToString();
            Price = GetPrice(book, saleStatus);
            ReleaseDate = book.ReleaseDate;
        }

        private static double GetPrice(Book book, Sale saleStatus) => saleStatus.Equals(Sale.Active) && book.IsOld() ? book.DiscountPrice : book.Price;
    }
}