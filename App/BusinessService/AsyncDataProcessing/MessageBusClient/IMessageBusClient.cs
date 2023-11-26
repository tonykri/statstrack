using BusinessService.Dto.MessageBus.Send;

namespace BusinessService.AsymcDataProcessing.MessageBusClient;

public interface IMessageBusClient
{
    void SendMessage(BusinessUpdatedDeletedDto business);
}