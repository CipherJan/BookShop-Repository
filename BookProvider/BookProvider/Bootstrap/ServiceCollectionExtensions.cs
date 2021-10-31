using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookProvider.Consumer;
using BookProvider.Core.MassTransit;

namespace BookProvider.Bootstrap
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassTransitPublisherAndConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            var massTransitConfig = new MassTransitConfiguration();
            configuration.GetSection("MassTransit").Bind(massTransitConfig);

            var queuqEndpoint = new QueueEndpoints();
            configuration.GetSection("QueueEndpoints").Bind(queuqEndpoint);

            services.AddScoped(_ => massTransitConfig);

            services.AddScoped(_ => queuqEndpoint);

            services.AddMassTransit(config => {

                config.AddConsumer<RequestConsumer>();

                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(massTransitConfig.RabbitMqAddress, hostConfigurator =>
                    {
                        hostConfigurator.Username(massTransitConfig.UserName);
                        hostConfigurator.Password(massTransitConfig.Password);
                    });
                    cfg.Durable = massTransitConfig.Durable;
                    cfg.PurgeOnStartup = massTransitConfig.PurgeOnStartup;
                    cfg.ReceiveEndpoint(queuqEndpoint.ReceiveQueueName, c => {
                        c.ConfigureConsumer<RequestConsumer>(ctx);
                    });
                });
            });
            return services;
        }
    }
}
