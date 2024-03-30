using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using AccountService.Dto.MessageBus.Send;

namespace UserService.AsymcDataProcessing.MessageBusClient;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConnection connection;
    private readonly IModel channel;
    public MessageBusClient(IConfiguration configuration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQHost"],
            Port = int.Parse(configuration["RabbitMQPort"])
        };
        try
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            Console.WriteLine("--> Connected to MessageBus");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
        }
    }

    public void Send<T>(ref T data)
    {
        if (data is not EmailNameCodeDto && data is not UserDeletedDto && data is not UserRegisteredDto && data is not UserUpdatedDto)
        {
            Console.WriteLine("Not accepted type of object");
            return;
        }
        var message = JsonSerializer.Serialize(data);

        if (connection.IsOpen)
        {
            Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
            SendMessage(message);
        }
        else
            Console.WriteLine("--> RabbitMQ connectionis closed, not sending");
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "trigger",
                        routingKey: "",
                        basicProperties: null,
                        body: body);
        Console.WriteLine($"--> We have sent {message}");
    }

    public void Dispose()
    {
        Console.WriteLine("MessageBus Disposed");
        if (channel.IsOpen)
        {
            channel.Close();
            connection.Close();
        }
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ Connection Shutdown");
    }
}
