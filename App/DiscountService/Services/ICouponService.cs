using DiscountService.Dto;

namespace DiscountService.Services;

public interface ICouponService {
    Task<ApiResponse<CouponDto, Exception>> Create(Guid businessId);
    Task<ApiResponse<int, Exception>> Redeem(Guid businessId, string code);
}