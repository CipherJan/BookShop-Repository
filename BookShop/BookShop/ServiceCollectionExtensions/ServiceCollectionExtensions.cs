using System.Collections.Specialized;
using BookShop.Jobs;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using BookShop.Core.MassTransit;
using ResponseConsumer = BookShop.Consumers.ResponseConsumer;

namespace BookShop.ServiceCollectionExtensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, InjectableJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>(isp =>
            {

                var properties = new NameValueCollection
                {
                    ["quartz.scheduler.interruptJobsOnShutdownWithWait"] = "true",
                    ["quartz.scheduler.interruptJobsOnShutdown"] = "true"
                };
                return new StdSchedulerFactory(properties);
            });
            services.AddSingleton<BooksOrderJob>();
            services.AddSingleton<MakeBooksOldJob>();
            services.AddHostedService<LibraryHostedService>();

            return services;
        }

        public static IServiceCollection AddMassTransitPublisherAndConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            var hostConfig = new MassTransitConfiguration();
            configuration.GetSection("MassTransit").Bind(hostConfig);

            services.AddMassTransit(config => {

                config.AddConsumer<ResponseConsumer>();

                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(hostConfig.RabbitMqAddress, hostConfiguration =>
                    {
                        hostConfiguration.Username(hostConfig.UserName);
                        hostConfiguration.Password(hostConfig.Password);
                    });
                    cfg.Durable = hostConfig.Durable;
                    cfg.PurgeOnStartup = hostConfig.PurgeOnStartup;
                    
                    cfg.ReceiveEndpoint("BookShop.ReceiveBooksEndpoint", c => {
                        c.ConfigureConsumer<ResponseConsumer>(ctx);
                    });
                });
            });
            return services;
        }
    }
}
