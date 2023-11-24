using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Repositories;
using Stripe;
using Stripe.Checkout;

namespace PaymentService.Endpoints;

public class PaymentEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("payment/pay", Payment)
            .RequireAuthorization("completed_profile");
        app.MapGet("payment/success", Success);
        app.MapGet("payment/cancelled",() => "Payment failed");
        
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IStripeRepo, StripeRepo>();
        services.AddScoped<IBusinessRepo, BusinessRepo>();
    }

    private IResult Success([FromServices] IBusinessRepo businessRepo, [FromQuery] string session_id, [FromQuery] string business_id, [FromQuery] Guid user_id)
    {
        try
        {
            var sessionService = new SessionService();
            var stripeSession = sessionService.Get(session_id);

            if(stripeSession.PaymentStatus.Equals("paid"))
            {
                if(business_id.Equals("new-business"))
                    businessRepo.CreateBusiness(user_id, session_id);
                else
                    businessRepo.RenewLicense(Guid.Parse(business_id), session_id);
            }
            else
                throw new Exception("Session failed");
            
            return Results.Ok(stripeSession);
        }
        catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch(PaymentExistsException ex)
        {
            return Results.Conflict(ex.Message);
        }
        catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
        
    }

    private IResult Payment([FromServices] IStripeRepo stripeRepo)
    {
        try
        {
            string url = stripeRepo.Pay(null);
            return Results.Redirect(url);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}