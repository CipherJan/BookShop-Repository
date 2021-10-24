using BookContractLibrary;

namespace BookShop.Services.Contracts
{
    public class RequestContract : IRequestContract
    {
        public int FromShopId { get; set; }
        public int NumberOfBooks { get; set; }
    }
}
