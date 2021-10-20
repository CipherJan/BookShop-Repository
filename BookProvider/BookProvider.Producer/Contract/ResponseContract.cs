using System.Collections.Generic;
using BookContractLibrary;


namespace BookProvider.Producer
{
    internal class ResponseContract : IResponseContract<BookContract>
    {
        public int ToShopId { get; set; }
        public double TotalBooksPrice { get; set; }
        public IEnumerable<BookContract> Books { get; set; }
    }
}
