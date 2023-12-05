namespace PaymentService.Repositories;

public interface IStripeRepo
{
    string Pay(Guid? businessId, string token);
}