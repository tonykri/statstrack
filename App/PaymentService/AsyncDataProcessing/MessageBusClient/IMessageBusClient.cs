using PaymentService.Dto.MessageBus.Send;

namespace PaymentService.AsymcDataProcessing.MessageBusClient;

public interface IMessageBusClient
{
    void SendMessage(BusinessCreatedRenewedDto coupon);
}