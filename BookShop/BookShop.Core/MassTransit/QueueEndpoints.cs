namespace BookShop.Core.MassTransit
{
    public class QueueEndpoints
    {
        public string ReceiveQueueName { get; set; }
        public string RequestQueueName { get; set; }
    }
}
