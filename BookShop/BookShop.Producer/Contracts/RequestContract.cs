using BookContractLibrary;

namespace BookShop.Producer.Contracts
{
    public class RequestContract : IRequestContract
    {
        public int FromShopId { get; set; }
        public int NumberOfBooks { get; set; }
    }
}
