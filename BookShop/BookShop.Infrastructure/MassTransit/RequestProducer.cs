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

        public RequestProducer(ISendEndpointProvider sendEndpointProvider, MassTransitConfiguration massTransitConfig)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _massTransitConfig = massTransitConfig;
        }
       
        public async Task SendBooksRequestEvent(IRequestContract request)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(_massTransitConfig.GetRequestEndpoint());
            await endpoint.Send(request);
        }
    }
}
