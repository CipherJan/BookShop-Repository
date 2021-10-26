using System.Collections.Generic;
using BookContractLibrary;

namespace BookProvider.Infrastructure.MassTransit.Contract
{
    internal class ResponseContract : IResponseContract
    {
        public int ToShopId { get; set; }
        public double TotalBooksPrice { get; set; }
        public IEnumerable<BookContract> Books { get; set; }
    }
}
