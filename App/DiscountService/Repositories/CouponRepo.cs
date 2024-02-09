using DiscountService.Dto;
using DiscountService.Models;
using DiscountService.Utils;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.Repositories;

public class CouponRepo : ICouponRepo
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    public CouponRepo(DataContext dataContext, ITokenDecoder tokenDecoder)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
    }

    public async Task<ApiResponse<List<CouponDto>, Exception>> GetAllUser()
    {
        Guid userId = tokenDecoder.GetUserId();
        var data = await dataContext.Coupons.Where(c => c.UserId == userId)
            .Select(c => new CouponDto
            {
                BusinessId = c.BusinessId,
                Brand = dataContext.Businesses.First(b => b.BusinessId == c.BusinessId).Brand,
                Code = c.Code,
                PurchaseDate = c.PurchaseDate,
                RedeemDate = c.RedeemDate
            }).ToListAsync();

        return new ApiResponse<List<CouponDto>, Exception>(data);
    }

    public async Task<ApiResponse<List<CouponDto>, Exception>> GetAllBusiness(Guid businessId)
    {
        Guid userId = tokenDecoder.GetUserId();
        if (!dataContext.Businesses.Any(b => b.BusinessId == businessId && b.UserId == userId))
            return new ApiResponse<List<CouponDto>, Exception>(new Exception(ExceptionMessages.BUSINESS_NOT_FOUND));

        var data = await dataContext.Coupons.Where(c => c.BusinessId == businessId && c.RedeemDate != null)
                .Select(c => new CouponDto
                {
                    BusinessId = null,
                    Brand = null,
                    Code = c.Code,
                    PurchaseDate = c.PurchaseDate,
                    RedeemDate = c.RedeemDate
                }).ToListAsync();

        return new ApiResponse<List<CouponDto>, Exception>(data);
    }

    public async Task<ApiResponse<Coupon, Exception>> GetCoupon(Guid businessId, string code)
    {
        Guid userId = tokenDecoder.GetUserId();
        if (!dataContext.Businesses.Any(b => b.BusinessId == businessId && b.UserId == userId))
            return new ApiResponse<Coupon, Exception>(new Exception(ExceptionMessages.BUSINESS_NOT_FOUND));

        var coupon = await dataContext.Coupons.FirstOrDefaultAsync(c => c.BusinessId == businessId && c.Code.Equals(code));
        if (coupon is null)
            return new ApiResponse<Coupon, Exception>(new Exception(ExceptionMessages.COUPON_NOT_FOUND));

        return new ApiResponse<Coupon, Exception>(coupon);
    }
}