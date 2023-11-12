using DiscountService.Dto;
using DiscountService.Models;

namespace DiscountService.Repositories;

public interface ICouponRepo
{
    CouponDto Create(Guid businessId);
    void Redeem(Guid businessId, string code);
    List<CouponDto> GetAllUser();
    List<CouponDto> GetAllBusiness(Guid businessId);
    Coupon GetCoupon(Guid businessId, string code);
}