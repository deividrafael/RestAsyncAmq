public class AmqSettings
{
    public string BrokerUri { get; set; } = "amqp://localhost:5672";
    public string QueueName { get; set; } = "client-id-queue";
}
