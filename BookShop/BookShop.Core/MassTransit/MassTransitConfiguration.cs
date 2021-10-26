namespace BookShop.Core.MassTransit
{ 
    public class MassTransitConfiguration
    {
        public string RabbitMqAddress { get; set; }
        
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Durable { get; set; }
        public bool PurgeOnStartup { get; set; }

    }
}
