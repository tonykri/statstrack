using DiscountService.Dto;
using DiscountService.Models;

namespace DiscountService.Repositories;

public interface ICouponRepo
{
    Task<ApiResponse<List<CouponDto>, Exception>> GetAllUser();
    Task<ApiResponse<List<CouponDto>, Exception>> GetAllBusiness(Guid businessId);
    Task<ApiResponse<Coupon, Exception>> GetCoupon(Guid businessId, string code);
}