using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Services;
using Stripe.Checkout;
using PaymentService.Dto;

namespace PaymentService.Endpoints;

public class PaymentEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("pay", Payment);
        app.MapGet("success", Success);
        app.MapGet("cancelled", () => "Payment failed");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IStripeService, StripeService>();
        services.AddScoped<IBusinessService, BusinessService>();
    }

    private async Task<IResult> Success([FromServices] IBusinessService businessService, [FromQuery] string session_id, [FromQuery] Guid? business_id, [FromQuery] Guid user_id)
    {
        var sessionService = new SessionService();
        var stripeSession = sessionService.Get(session_id);

        if (!stripeSession.PaymentStatus.Equals("paid"))
            return Results.BadRequest("Session failed");

        ApiResponse<int, Exception> result;
        if (business_id is null)
            result = await businessService.CreateBusiness(user_id, session_id);
        else
            result = await businessService.RenewLicense(business_id, session_id);

        return result.Match(
            data => Results.Ok(stripeSession),
            exception => Results.BadRequest()
        );
    }

    private IResult Payment([FromServices] IStripeService stripeService, [FromQuery] string? token, [FromQuery] Guid? businessId)
    {
        if (token is null)
            return Results.BadRequest("Token is required");

        var result = stripeService.Pay(businessId, token);
        return result.Match(
            data =>
            {
                if (data is not null)
                    return Results.Redirect(data);
                return Results.BadRequest();
            },
            exception => Results.Forbid()
        );
    }
}