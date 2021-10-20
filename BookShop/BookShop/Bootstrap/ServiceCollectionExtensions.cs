using System.Collections.Specialized;
using BookShop.Jobs;
using Core;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using BookShop.Consumer;
using MassTransit.MultiBus;
using BookShop.Bootstrap.Interface;

namespace BookShop.Bootstrap
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
            var hostConfig = new MassTransitConfiguration();
            configuration.GetSection("MassTransit").Bind(hostConfig);

            services.AddMassTransit(config => {

                config.AddConsumer<ResponceConsumer>();

                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(hostConfig.RabbitMqAddress, configuration =>
                    {
                        configuration.Username(hostConfig.UserName);
                        configuration.Password(hostConfig.Password);
                    });
                    cfg.Durable = hostConfig.Durable;
                    cfg.PurgeOnStartup = hostConfig.PurgeOnStartup;
                    cfg.ReceiveEndpoint("Responce-Queue", c => {
                        c.ConfigureConsumer<ResponceConsumer>(ctx);
                    });
                });
            });

            services.AddMassTransit<ISecondBus>(config => {

                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(hostConfig.RabbitMqAddress, configuration =>
                    {
                        configuration.Username(hostConfig.UserName);
                        configuration.Password(hostConfig.Password);
                    });
                    cfg.Durable = hostConfig.Durable;
                    cfg.PurgeOnStartup = hostConfig.PurgeOnStartup;
                });
            });

            return services;
        }
    }
}
