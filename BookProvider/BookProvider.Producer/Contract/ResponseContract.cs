using System.Collections.Generic;
using BookContractLibrary;

namespace BookProvider.Producer.Contract
{
    internal class ResponseContract : IResponseContract<BookContract>
    {
        public int ToShopId { get; set; }
        public double TotalBooksPrice { get; set; }
        public IEnumerable<BookContract> Books { get; set; }
    }
}
