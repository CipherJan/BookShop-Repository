using System.Collections.Generic;

namespace BookShop.Core.Entities
{
    public static class Discounts
    {
        private const double FantasticDiscountInPercent = 0.03;
        private const double AdventuresDiscountInPercent = 0.07;
        private const double EncyclopediaDiscountInPercent = 0.1;

        public static readonly Dictionary<BookGenre, double> GenresAndDiscounts = new()
        {
            {BookGenre.Adventure, AdventuresDiscountInPercent},
            {BookGenre.Encyclopedia, EncyclopediaDiscountInPercent},
            {BookGenre.Fantastic, FantasticDiscountInPercent}
        };
    }
}
