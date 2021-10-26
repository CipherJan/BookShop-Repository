using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Specialized;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using BookShop.Core.MassTransit;
using BookShop.Consumers;
using BookShop.Jobs;

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
            services.AddHostedService<LibraryHostedService>();

            return services;
        }

        public static IServiceCollection AddMassTransitPublisherAndConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            var massTransitConfig = new MassTransitConfiguration();
            configuration.GetSection("MassTransit").Bind(massTransitConfig);

            services.AddScoped(isp =>
            {
                return massTransitConfig;
            });

            services.AddMassTransit(config => {

                config.AddConsumer<ResponseConsumer>();

                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(massTransitConfig.RabbitMqAddress, hostConfiguration =>
                    {
                        hostConfiguration.Username(massTransitConfig.UserName);
                        hostConfiguration.Password(massTransitConfig.Password);
                    });
                    cfg.Durable = massTransitConfig.Durable;
                    cfg.PurgeOnStartup = massTransitConfig.PurgeOnStartup;
                    
                    cfg.ReceiveEndpoint(massTransitConfig.ReceiveQueueName, c => {
                        c.ConfigureConsumer<ResponseConsumer>(ctx);
                    });
                });
            });
            return services;
        }
    }
}

