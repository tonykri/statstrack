using PaymentService.Dto;

namespace PaymentService.Services;

public interface IStripeService
{
    ApiResponse<string, Exception> Pay(Guid? businessId, string token);
}