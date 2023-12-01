using ReviewService.Dto.MessageBus.Send;

namespace ReviewService.AsymcDataProcessing.MessageBusClient;

public interface IMessageBusClient
{
    void UpdateStars(BusinessStarsDto data);
}