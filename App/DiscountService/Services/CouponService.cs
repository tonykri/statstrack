using DiscountService.AsymcDataProcessing.MessageBusClient;
using DiscountService.Dto;
using DiscountService.Dto.MessageBus.Send;
using DiscountService.Models;
using DiscountService.Utils;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.Services;

public class CouponService : ICouponService {
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    private readonly IMessageBusClient messageBusClient;
    public CouponService(DataContext dataContext, ITokenDecoder tokenDecoder, IMessageBusClient messageBusClient)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
        this.messageBusClient = messageBusClient;
    }

    private string CodeGenerator()
    {
        string characterSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        Random random = new Random();

        char[] code = new char[6];
        for (int i = 0; i < 6; i++)
        {
            int index = random.Next(characterSet.Length);
            code[i] = characterSet[index];
        }
        return new string(code);
    }

    public async Task<ApiResponse<CouponDto, Exception>> Create(Guid businessId)
    {
        Guid userId = tokenDecoder.GetUserId();
        var business = dataContext.Businesses.FirstOrDefault(b => b.BusinessId == businessId);
        if (business is null)
            return new ApiResponse<CouponDto, Exception>(new Exception(ExceptionMessages.BUSINESS_NOT_FOUND));
        if (dataContext.Businesses.Any(b => b.BusinessId == businessId && b.UserId == userId))
            return new ApiResponse<CouponDto, Exception>(new Exception(ExceptionMessages.UNAUTHORIZED));
        if (dataContext.Coupons.Any(c => c.UserId == userId && c.BusinessId == businessId && c.PurchaseDate.AddDays(3) > DateTime.Now))
            return new ApiResponse<CouponDto, Exception>(new Exception(ExceptionMessages.UNAUTHORIZED));

        string code = CodeGenerator();
        while (dataContext.Coupons.Any(c => c.Code.Equals(code) && c.BusinessId == businessId && c.PurchaseDate.AddDays(3) > DateTime.Now))
            code = CodeGenerator();

        Coupon coupon = new Coupon
        {
            UserId = userId,
            BusinessId = businessId,
            Business = business,
            Code = code
        };
        await dataContext.AddAsync(coupon);
        await dataContext.SaveChangesAsync();
        var data = new CouponDto
        {
            BusinessId = businessId,
            Brand = business.Brand,
            Code = code,
            PurchaseDate = coupon.PurchaseDate,
            RedeemDate = coupon.RedeemDate
        };

        return new ApiResponse<CouponDto, Exception>(data);
    }

    public async Task<ApiResponse<int, Exception>> Redeem(Guid businessId, string code)
    {
        Guid userId = tokenDecoder.GetUserId();
        var business = await dataContext.Businesses.FirstOrDefaultAsync(b => b.BusinessId == businessId && b.UserId == userId);
        if (business is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.BUSINESS_NOT_FOUND));
        var coupon = await dataContext.Coupons.FirstOrDefaultAsync(c => c.Code.Equals(code) && c.BusinessId == business.BusinessId);
        if (coupon is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.COUPON_NOT_FOUND));

        if (coupon.PurchaseDate.AddDays(3) < DateTime.Now && coupon.RedeemDate is not null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.COUPON_NOT_VALID));

        coupon.RedeemDate = DateTime.Now;
        await dataContext.SaveChangesAsync();
        messageBusClient.CouponRedeem(new CouponRedeemedDto(userId, business.BusinessId));

        return new ApiResponse<int, Exception>(0);
    }
}