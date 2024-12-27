using Apache.NMS;
using Apache.NMS.AMQP;

public class AmqProducer : IAmqProducer
{
    private readonly AmqSettings? _settings;

    public AmqProducer(IConfiguration configuration)
    {
        _settings = configuration.GetSection("AmqSettings").Get<AmqSettings>();
    }

    public async Task SendMessageAsync(string message)
    {
        var factory = new ConnectionFactory();
        if (_settings == null)
        {
            throw new InvalidOperationException("AMQ settings are not configured.");
        }

        using var connection = factory.CreateConnection(_settings.BrokerUri, null);
        using var session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
        var destination = session.GetQueue(_settings.QueueName);
        using var producer = session.CreateProducer(destination);

        var textMessage = session.CreateTextMessage(message);
        await Task.Run(() => producer.Send(textMessage));

        Console.WriteLine($"Message sent to AMQ: {message}");
    }
}
