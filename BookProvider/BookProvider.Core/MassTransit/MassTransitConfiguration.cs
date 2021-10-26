using System;

namespace BookProvider.Core.MassTransit
{ 
    public class MassTransitConfiguration
    {
        public string RabbitMqAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Durable { get; set; }
        public bool PurgeOnStartup { get; set; }
        public string ReceiveQueueName { get; set; }
        public string RequestQueueName { private get; set; }

        public Uri GetQueueAddress()
        {
            return new Uri(RabbitMqAddress + "/" + RequestQueueName);
        }
    }
}
