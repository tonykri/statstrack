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
        app.MapGet("pay", Payment);
        app.MapGet("success", Success);
        app.MapGet("cancelled",() => "Payment failed");
        
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IStripeRepo, StripeRepo>();
        services.AddScoped<IBusinessRepo, BusinessRepo>();
    }

    private IResult Success([FromServices] IBusinessRepo businessRepo, [FromQuery] string session_id, [FromQuery] Guid? business_id, [FromQuery] Guid user_id)
    {
        try
        {
            var sessionService = new SessionService();
            var stripeSession = sessionService.Get(session_id);

            if(stripeSession.PaymentStatus.Equals("paid"))
            {
                if(business_id is null)
                    businessRepo.CreateBusiness(user_id, session_id);
                else
                    businessRepo.RenewLicense(business_id, session_id);
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

    private IResult Payment([FromServices] IStripeRepo stripeRepo, [FromQuery] string? token, [FromQuery] Guid? businessId)
    {
        try
        {
            if (token is null)
                throw new Exception("Token is null");
            string url = stripeRepo.Pay(businessId, token);
            return Results.Redirect(url);
        }catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Unauthorized();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}