using DiscountService.AsymcDataProcessing.MessageBusClient;
using DiscountService.Dto;
using DiscountService.Dto.MessageBus.Send;
using DiscountService.Models;
using DiscountService.Utils;

namespace DiscountService.Repositories;

public class CouponRepo : ICouponRepo
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    private readonly IMessageBusClient messageBusClient;
    public CouponRepo(DataContext dataContext, ITokenDecoder tokenDecoder, IMessageBusClient messageBusClient)
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

    public CouponDto Create(Guid businessId)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var business = dataContext.Businesses.FirstOrDefault(b => b.BusinessId == businessId);
            if(business is null)
                throw new NotFoundException("Business not found");
            if(dataContext.Businesses.Any(b => b.BusinessId == businessId && b.UserId == userId))
                throw new NotAllowedException("Owners are not allowed to get discount coupon");
            if(dataContext.Coupons.Any(c => c.UserId == userId && c.BusinessId == businessId && c.PurchaseDate.AddDays(3) > DateTime.Now))
                throw new NotAllowedException("You can get a coupon every 3 days from a business");
            
            string code = CodeGenerator();
            while(dataContext.Coupons.Any(c => c.Code.Equals(code) && c.BusinessId == businessId && c.PurchaseDate.AddDays(3) > DateTime.Now))
                code = CodeGenerator();

            Coupon coupon = new Coupon
            {
                UserId = userId,
                BusinessId = businessId,
                Business = business,
                Code = code
            };
            dataContext.Add(coupon);
            dataContext.SaveChanges();
            return new CouponDto{
                BusinessId = businessId,   
                Brand = business.Brand,
                Code = code,
                PurchaseDate = coupon.PurchaseDate,
                RedeemDate = coupon.RedeemDate
            };
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public List<CouponDto> GetAllUser()
    {
        Guid userId = tokenDecoder.GetUserId();
        return dataContext.Coupons.Where(c => c.UserId == userId)
            .Select(c => new CouponDto{
                BusinessId = c.BusinessId,
                Brand = dataContext.Businesses.First(b => b.BusinessId == c.BusinessId).Brand,
                Code = c.Code,
                PurchaseDate = c.PurchaseDate,
                RedeemDate = c.RedeemDate
        }).ToList();
    }

    public List<CouponDto> GetAllBusiness(Guid businessId)
    {
        Guid userId = tokenDecoder.GetUserId();
        if(!dataContext.Businesses.Any(b => b.BusinessId == businessId && b.UserId == userId))
            throw new NotFoundException("Business for current user not found");

        return dataContext.Coupons.Where(c => c.BusinessId == businessId && c.RedeemDate != null)
                .Select(c => new CouponDto{
                    BusinessId = null,
                    Brand = null,
                    Code = c.Code,
                    PurchaseDate = c.PurchaseDate,
                    RedeemDate = c.RedeemDate
            }).ToList();
    }

    public Coupon GetCoupon(Guid businessId, string code)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            if(!dataContext.Businesses.Any(b => b.BusinessId == businessId && b.UserId == userId))
                throw new NotFoundException("Business for current user not found");

            var coupon = dataContext.Coupons.FirstOrDefault(c => c.BusinessId == businessId && c.Code.Equals(code));
            if (coupon is null)
                throw new NotFoundException("Coupon not found");
            return coupon;
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void Redeem(Guid businessId, string code)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var business = dataContext.Businesses.FirstOrDefault(b => b.BusinessId == businessId && b.UserId == userId);
            if(business is null)
                throw new NotFoundException("Business not found");
            var coupon = dataContext.Coupons.FirstOrDefault(c => c.Code.Equals(code) && c.BusinessId == business.BusinessId);
            if(coupon is null)
                throw new NotFoundException("Discount coupon not found");

            if(coupon.PurchaseDate.AddDays(3) < DateTime.Now && coupon.RedeemDate is not null)
                throw new NotValidException("Discount coupon not valid");
            
            coupon.RedeemDate = DateTime.Now;
            dataContext.SaveChanges();
            messageBusClient.SendMessage(new CouponRedeemedDto(userId, business.BusinessId));
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}