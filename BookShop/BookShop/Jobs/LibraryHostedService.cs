﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using JetBrains.Annotations;
using Quartz;
using Quartz.Spi;

namespace BookShop.Jobs
{
    [UsedImplicitly]
    public class LibraryHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private static IScheduler Scheduler { get; set; }

        public LibraryHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;
            await ConfigureBooksOrderJob();
            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
            Scheduler = null;
        }

        private async Task ConfigureBooksOrderJob()
        {
            var trigger = TriggerBuilder.Create()
                .WithIdentity(nameof(BooksOrderJob)).StartNow() 
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();
            var job = JobBuilder.Create<BooksOrderJob>().WithIdentity(nameof(BooksOrderJob)).Build();
            await Scheduler.ScheduleJob(job, trigger);
        }
    }
}
;


