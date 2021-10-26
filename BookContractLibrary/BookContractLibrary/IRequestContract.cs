namespace BookContractLibrary
{
    public interface IRequestContract
    {
        int FromShopId { get; set; }
        int NumberOfBooks { get; set; }
    }
}
