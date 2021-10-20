using System.Threading.Tasks;

namespace BookShop.Producer.Interface
{
    public interface IRequestProduser
    {
        public Task SendBooksRequestEvent(int fromShopId, int numberofBooks);
    }
}
