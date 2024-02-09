using Config.Stracture;
using DiscountService.Repositories;
using DiscountService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiscountService.Endpoints;

public class CouponEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("coupon/{businessId:guid}", Create)
            .RequireAuthorization("completed_profile");
        app.MapGet("coupon/redeem/{businessId:guid}/{code}", Redeem)
            .RequireAuthorization("completed_profile");
        app.MapGet("coupon/all", GetAllUser)
            .RequireAuthorization("completed_profile");
        app.MapGet("coupon/business/all/{businessId:guid}", GetAllBusiness)
            .RequireAuthorization("completed_profile");
        app.MapGet("coupon/business/{businessId:guid}/{code}", GetCoupon)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<ICouponRepo, CouponRepo>();
        services.AddScoped<ICouponService, CouponService>();
    }

    private async Task<IResult> Create([FromServices] ICouponService couponService, [FromRoute] Guid businessId)
    {
        var result = await couponService.Create(businessId);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception =>
            {
                if (exception?.Message == ExceptionMessages.UNAUTHORIZED)
                    return Results.Unauthorized();
                return Results.NotFound(exception?.Message);
            }
        );
    }

    private async Task<IResult> Redeem([FromServices] ICouponService couponService, [FromRoute] Guid businessId, [FromRoute] string code)
    {
        var result = await couponService.Redeem(businessId, code);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception =>
            {
                if (exception?.Message == ExceptionMessages.COUPON_NOT_VALID)
                    return Results.BadRequest(exception?.Message);
                return Results.NotFound(exception?.Message);
            }
        );
    }

    private async Task<IResult> GetAllUser([FromServices] ICouponRepo couponRepo)
    {
        var result = await couponRepo.GetAllUser();
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> GetAllBusiness([FromServices] ICouponRepo couponRepo, [FromRoute] Guid businessId)
    {
        var result = await couponRepo.GetAllBusiness(businessId);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.NotFound(exception?.Message)
        );
    }

    private async Task<IResult> GetCoupon([FromServices] ICouponRepo couponRepo, [FromRoute] Guid businessId, [FromRoute] string code)
    {
        var result = await couponRepo.GetCoupon(businessId, code);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.NotFound(exception?.Message)
        );
    }
}