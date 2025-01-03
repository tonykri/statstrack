using PaymentService.Dto;
using PaymentService.Utils;
using Stripe;
using Stripe.Checkout;

namespace PaymentService.Services;

public class StripeService : IStripeService
{
    private readonly ITokenDecoder tokenDecoder;
    public StripeService(ITokenDecoder tokenDecoder)
    {
        this.tokenDecoder = tokenDecoder;
    }
    public ApiResponse<string, Exception> Pay(Guid? businessId, string token)
    {
        if (!tokenDecoder.GetClaims(token)["ProfileStage"].Equals("Completed"))
            return new ApiResponse<string, Exception>(new Exception(ExceptionMessages.UNAUTHORIZED));

        string businessUrl = businessId is null ? "" : "&business_id=" + businessId;
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
            SuccessUrl = "http://10.0.2.2:4005/success?session_id={CHECKOUT_SESSION_ID}" + "&user_id=" + tokenDecoder.GetUserId(token) + businessUrl,
            CancelUrl = "http://10.0.2.2:4005/cancelled",
        };

        var sessionService = new SessionService();
        var session = sessionService.Create(checkoutSessionOptions);

        return new ApiResponse<string, Exception>(session.Url);
    }
}
