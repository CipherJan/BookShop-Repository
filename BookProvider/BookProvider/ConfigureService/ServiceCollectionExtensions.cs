using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using MassTransit.MultiBus;
using Core;
using BookProvider.Consumer;

namespace ConfigureService
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassTransitPublisherAndConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            var hostConfig = new MassTransitConfiguration();
            configuration.GetSection("MassTransit").Bind(hostConfig);

            services.AddMassTransit(config => {

                config.AddConsumer<RequestConsumer>();

                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(hostConfig.RabbitMqAddress, configuration =>
                    {
                        configuration.Username(hostConfig.UserName);
                        configuration.Password(hostConfig.Password);
                    });
                    cfg.Durable = hostConfig.Durable;
                    cfg.PurgeOnStartup = hostConfig.PurgeOnStartup;
                    cfg.ReceiveEndpoint("Request-Queue", c => {
                        c.ConfigureConsumer<RequestConsumer>(ctx);
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
