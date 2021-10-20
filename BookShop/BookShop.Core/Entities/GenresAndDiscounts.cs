using System.Collections.Generic;

namespace BookShop.Core.Entities
{
    public static class Discounts
    {
        private const double FANTASTIC_DISCOUNT_IN_PERCENT = 0.03;
        private const double ADVENTURES_DISCOUNT_IN_PERCENT = 0.07;
        private const double ENCYCLOPEDIA_DISCOUNT_IN_PERCENT = 0.1;

        private static readonly Dictionary<BookGenre, double> _genresAndDiscounts = new()
        {
            {BookGenre.Adventures, ADVENTURES_DISCOUNT_IN_PERCENT},
            {BookGenre.Encyclopedia, ENCYCLOPEDIA_DISCOUNT_IN_PERCENT},
            {BookGenre.Fantastic, FANTASTIC_DISCOUNT_IN_PERCENT}
        };

        public static Dictionary<BookGenre, double> GenresAndDiscounts
        {
            get => _genresAndDiscounts;
        }
    }
}
