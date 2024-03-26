using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace UserService.AsymcDataProcessing;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration configuration;
    private IConnection connection;
    private IModel channel;
    private string queueName;
    public MessageBusSubscriber(IConfiguration configuration)
    {
        this.configuration = configuration;

        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory() { HostName = configuration["RabbitMQHost"], Port = int.Parse(configuration["RabbitMQPort"]) };

        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(queue: queueName,
            exchange: "trigger",
            routingKey: "");

        Console.WriteLine("--> Listenting on the Message Bus...");

        connection.ConnectionShutdown += RabbitMQ_ConnectionShitdown;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (ModuleHandle, ea) =>
        {
            Console.WriteLine("--> Event Received!");
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    private void RabbitMQ_ConnectionShitdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> Connection Shutdown");
    }

    public override void Dispose()
    {
        if (channel.IsOpen)
        {
            channel.Close();
            connection.Close();
        }

        base.Dispose();
    }
}