using System.Collections.Generic;

namespace BookContractLibrary
{
    public interface IResponseContract
    {
        int ToShopId { get; set; }
        double TotalBooksPrice { get; set; }
        IEnumerable<BookContract> Books { get; set; }
    }
}
