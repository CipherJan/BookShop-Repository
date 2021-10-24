using System;
using System.Threading.Tasks;
using BookContractLibrary;
using BookShop.Infrastructure.MassTransit.Interface;
using MassTransit;

namespace BookShop.Infrastructure.MassTransit
{
    public class RequestProducer : IRequestProducer
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        
        public RequestProducer(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }
       
        public async Task SendBooksRequestEvent(IRequestContract request)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/BookProvider.RequestBooksEndpoint"));
            await endpoint.Send(request);
        }
    }
}
