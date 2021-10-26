using System;
using BookShop.Core.Entities;

namespace BookShop.Core.Models.BookModel
{
    public class BookModelState
    {
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Genre { get; private set; }
        public double Price { get; private set; }
        public string Novelty { get; private set; }
        public string ReleaseDate { get; private set; }
        public string BookId { get; private set; }

        public BookModelState(Book book, ShopSale saleStatus)
        {
            Title = book.Title;
            Author = book.Author;
            Genre = book.Genre.ToString();
            Novelty = GetNovelty(book.IsNew());
            Price = GetPrice(book, saleStatus);
            ReleaseDate = book.ReleaseDate.ToString();
            BookId = book.Id.ToString();
        }
        private string GetNovelty(bool value) => value ? "New" : "Old";
        private double GetPrice(Book book, ShopSale saleStatus) => saleStatus.Equals(ShopSale.Active) && !book.IsNew() ? book.DiscountPrice : book.Price;
    }
}
