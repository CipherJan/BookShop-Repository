using System;
using System.Threading.Tasks;
using MassTransit;
using BookContractLibrary;
using BookShop.Core.MassTransit;
using BookShop.Infrastructure.MassTransit.Interface;

namespace BookShop.Infrastructure.MassTransit
{
    public class RequestProducer : IRequestProducer
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly MassTransitConfiguration _massTransitConfig;
        private readonly QueueEndpoints _queueEndpoints;

        public RequestProducer(ISendEndpointProvider sendEndpointProvider, MassTransitConfiguration massTransitConfig, QueueEndpoints queueEndpoints)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _massTransitConfig = massTransitConfig;
            _queueEndpoints = queueEndpoints;
        }
       
        public async Task SendBooksRequestEvent(IRequestContract request)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"{_massTransitConfig.RabbitMqAddress}/{_queueEndpoints.RequestQueueName}"));
            await endpoint.Send(request);
        }
    }
}
