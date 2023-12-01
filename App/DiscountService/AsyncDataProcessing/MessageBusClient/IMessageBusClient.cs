using DiscountService.Dto.MessageBus.Send;

namespace DiscountService.AsymcDataProcessing.MessageBusClient;

public interface IMessageBusClient
{
    void CouponRedeem(CouponRedeemedDto coupon);
}