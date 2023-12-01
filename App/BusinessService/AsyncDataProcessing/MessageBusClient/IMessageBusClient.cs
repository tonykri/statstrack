using BusinessService.Dto.MessageBus.Send;

namespace BusinessService.AsymcDataProcessing.MessageBusClient;

public interface IMessageBusClient
{
    void UpdateBusiness(BusinessUpdatedDto business);
    void DeleteBusiness(BusinessDeletedDto business);
}