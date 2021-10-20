using System;
using BookShop.Core.Models;

namespace BookShop.Core.Entities
{
    public class Book
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public BookGenre Genre { get; private set; }

        private double _price;
        public double Price
        {
            get => _price;
            private set
            {
                _price = value;
                DiscountPrice = value;
            }
        }
        private double _discountPrice;
        public double DiscountPrice
        {
            get => _discountPrice;
            private set
            {
                _discountPrice = value;

                foreach (var (genre, discount) in Discounts.GenresAndDiscounts)
                {
                    if (Genre.Equals(genre))
                    {
                        _discountPrice = value * (1.0 - discount);
                        break;
                    }
                }
            }
        }
        public DateTime ReleaseDate { get; private set; }

        public BookNovelty Novelty { get; private set; }

        public int ShopId { get; private set; }
        public Shop Shop { get; private set; }

        private Book()
        {
        }

        public Book(BookModel model)
        {
            Title = model.Title;
            Author = model.Author;
            Genre = Enum.Parse<BookGenre>(model.Genre);
            Price = model.Price;
            ReleaseDate = model.ReleaseDate;
            Novelty = BookNovelty.New;
        }

        public void MakeOld()
        {
            Novelty = BookNovelty.Old;
        }

    }
}
