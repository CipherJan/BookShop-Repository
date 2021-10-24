using BookShop.Core.Entities;

namespace BookShop.Core.Models.ShopModel
{
    public class ShopModelState
    {
        public string Name { get; set; }
        public double Balance { get; set; }
        public int MaxBookQuantity { get; set; }
        public int BooksCount { get; set; }

        public int ShopId { get; set; }
        
        public ShopModelState(Shop shop)
        {
            Name = shop.Name;
            Balance = shop.Balance;
            MaxBookQuantity = shop.MaxBookQuantity;
            BooksCount = shop.Books.Count;
            ShopId = shop.Id;
        }
    }
}
