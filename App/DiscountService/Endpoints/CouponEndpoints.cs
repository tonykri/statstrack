using Config.Stracture;
using DiscountService.Dto;
using DiscountService.Models;
using DiscountService.Repositories;
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
    }

    private IResult Create([FromServices] ICouponRepo couponRepo, [FromRoute] Guid businessId)
    {
        try
        {
            CouponDto data = couponRepo.Create(businessId);
            return Results.Ok(data);
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(NotAllowedException ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Forbid();
        }
    }

    private IResult Redeem([FromServices] ICouponRepo couponRepo, [FromRoute] Guid businessId, [FromRoute] string code)
    {
        try
        {
            couponRepo.Redeem(businessId, code);
            return Results.NoContent();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(NotValidException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult GetAllUser([FromServices] ICouponRepo couponRepo)
    {
        List<CouponDto> coupons = couponRepo.GetAllUser();
        return Results.Ok(coupons);
    }

    private IResult GetAllBusiness([FromServices] ICouponRepo couponRepo, [FromRoute] Guid businessId)
    {
        try
        {
            List<CouponDto> coupons = couponRepo.GetAllBusiness(businessId);
            return Results.Ok(coupons);
        }catch(NotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            return Results.NotFound(ex.Message);
        }        
    }

    private IResult GetCoupon([FromServices] ICouponRepo couponRepo, [FromRoute] Guid businessId, [FromRoute] string code)
    {
        try
        {
            Coupon coupon = couponRepo.GetCoupon(businessId, code);
            return Results.Ok(coupon);
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
    }
}