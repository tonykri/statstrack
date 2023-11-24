using PaymentService.Utils;
using Stripe;
using Stripe.Checkout;

namespace PaymentService.Repositories;

public class StripeRepo : IStripeRepo
{
    private readonly ITokenDecoder tokenDecoder;
    public StripeRepo(ITokenDecoder tokenDecoder)
    {
        this.tokenDecoder = tokenDecoder;
    }
    public string Pay(Guid? businessId)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = 1200,
                Currency = "eur",
                PaymentMethodTypes = new List<string> { "card" }
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            var checkoutSessionOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = "price_1OFuXSGeii3zzoSa423BKBia",
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = "http://localhost:4005/payment/success?session_id={CHECKOUT_SESSION_ID}&business_id=" 
                    + businessId is null ? "new-business" : businessId
                    + "user_id=" + tokenDecoder.GetUserId(),
                CancelUrl = "http://localhost:4005/payment/cancelled",
            };

            var sessionService = new SessionService();
            var session = sessionService.Create(checkoutSessionOptions);

            return session.Url;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}