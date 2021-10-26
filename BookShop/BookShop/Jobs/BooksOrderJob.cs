using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using JetBrains.Annotations;
using Quartz;
using BookShop.Services.Interfaces.Services;

namespace BookShop.Jobs
{
    [UsedImplicitly]
    [DisallowConcurrentExecution]
    public class BooksOrderJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public BooksOrderJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var shopService = scope.ServiceProvider.GetRequiredService<IShopService>();
            await shopService.OrderBooksForAllShops();
        }
    }
}
