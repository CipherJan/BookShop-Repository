using System.Threading.Tasks;
using JetBrains.Annotations;
using Quartz;
using BookShop.Services.Interfaces.Services;

namespace BookShop.Jobs
{
    public class MakeBooksOldJob: IJob
    {
        private readonly IShopService _shopService;

        public MakeBooksOldJob(IShopService shopService)
        {
            _shopService = shopService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _shopService.JobMakeBooksOld();
        }
    }
}
