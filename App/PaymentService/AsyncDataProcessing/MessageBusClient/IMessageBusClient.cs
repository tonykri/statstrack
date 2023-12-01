using PaymentService.Dto.MessageBus.Send;

namespace PaymentService.AsymcDataProcessing.MessageBusClient;

public interface IMessageBusClient
{
    void BusinessCreateRenew(BusinessCreatedRenewedDto coupon);
}