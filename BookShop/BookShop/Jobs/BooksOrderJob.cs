using System.Threading.Tasks;
using JetBrains.Annotations;
using Quartz;
using BookShop.Services.Interfaces.Services;

namespace BookShop.Jobs
{
    [UsedImplicitly]
    [DisallowConcurrentExecution]
    public class BooksOrderJob : IJob
    {
        private readonly IShopService _shopService;

        public BooksOrderJob(IShopService shopService)
        {
            _shopService = shopService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _shopService.JobBookOrder();
        }
    }

}
