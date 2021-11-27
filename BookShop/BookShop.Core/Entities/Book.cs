using System;
using BookShop.Core.Models.BookModel;
using JetBrains.Annotations;

namespace BookShop.Core.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Book
    {
        private double _discountPrice;
        private double _price;

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public BookGenre Genre { get; private set; }
        
        public double Price
        {
            get => _price;
            private init
            {
                _price = value;
                DiscountPrice = value;
            }
        }
        
        public double DiscountPrice
        {
            get => _discountPrice;
            private init
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
        public BookSaleStatus Status { get; private set; }
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
            Status = BookSaleStatus.Asale;
        }

        public bool IsNew()
        {
            return DateTime.Now < ReleaseDate.AddYears(1);
        }

        public void Sold()
        {
            Status = BookSaleStatus.Sold;
        }

        public double GetCurrentPrice(ShopSale sale)
        {
            return sale.Equals(ShopSale.Active) && !IsNew() ? DiscountPrice : Price;
        }
    }
}
