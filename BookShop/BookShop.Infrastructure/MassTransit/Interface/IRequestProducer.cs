using System.Threading.Tasks;
using BookContractLibrary;

namespace BookShop.Infrastructure.MassTransit.Interface
{
    public interface IRequestProducer
    {
        Task SendBooksRequestEvent(IRequestContract request);
    }
}