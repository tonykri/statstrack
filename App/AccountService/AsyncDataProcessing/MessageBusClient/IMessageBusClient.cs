namespace UserService.AsymcDataProcessing.MessageBusClient;

public interface IMessageBusClient
{
    void Send<T>(ref T data);
}