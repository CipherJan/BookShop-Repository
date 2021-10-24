using BookProvider.Consumer;
using BookProvider.Core;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookProvider.Bootstrap
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
                    cfg.Host(hostConfig.RabbitMqAddress, hostConfigurator =>
                    {
                        hostConfigurator.Username(hostConfig.UserName);
                        hostConfigurator.Password(hostConfig.Password);
                    });
                    cfg.Durable = hostConfig.Durable;
                    cfg.PurgeOnStartup = hostConfig.PurgeOnStartup;
                    cfg.ReceiveEndpoint("BookProvider.RequestBooksEndpoint", c => {
                        c.ConfigureConsumer<RequestConsumer>(ctx);
                    });
                });
            });
            return services;
        }
    }
}
