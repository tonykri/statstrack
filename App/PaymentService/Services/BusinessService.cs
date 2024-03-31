
using Microsoft.EntityFrameworkCore;
using PaymentService.AsymcDataProcessing.MessageBusClient;
using PaymentService.Dto;
using PaymentService.Dto.MessageBus.Send;
using PaymentService.Models;

namespace PaymentService.Services;

public class BusinessService : IBusinessService
{
    private readonly DataContext dataContext;
    private readonly IMessageBusClient messageBusClient;
    public BusinessService(DataContext dataContext, IMessageBusClient messageBusClient)
    {
        this.dataContext = dataContext;
        this.messageBusClient = messageBusClient;
    }
    public async Task<ApiResponse<int, Exception>> CreateBusiness(Guid user_id, string session_id)
    {
        if (dataContext.Payments.Any(p => p.Id.Equals(session_id)))
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.PAYMENT_EXISTS));

        Business business = new Business
        {
            UserId = user_id,
            ExpirationDate = DateTime.UtcNow.AddYears(1)
        };
        Payment payment = new Payment
        {
            Id = session_id,
            Business = business,
            BusinessId = business.BusinessId
        };

        await dataContext.AddAsync(business);
        await dataContext.AddAsync(payment);
        await dataContext.SaveChangesAsync();
        messageBusClient.BusinessCreateRenew(new BusinessCreatedRenewedDto(business.BusinessId, business.UserId, business.ExpirationDate, "Business_Created"));

        return new ApiResponse<int, Exception>(0);
    }

    public async Task<ApiResponse<int, Exception>> RenewLicense(Guid? business_id, string session_id)
    {
        if (dataContext.Payments.Any(p => p.Id.Equals(session_id)))
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.PAYMENT_EXISTS));

        var business = await dataContext.Businesses.FirstOrDefaultAsync(b => b.BusinessId == business_id);
        if (business is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.BUSINESS_NOT_FOUND));

        Payment payment = new Payment
        {
            Id = session_id,
            Business = business,
            BusinessId = business.BusinessId
        };
        business.ExpirationDate = business.ExpirationDate.AddYears(1);

        await dataContext.AddAsync(payment);
        await dataContext.SaveChangesAsync();
        messageBusClient.BusinessCreateRenew(new BusinessCreatedRenewedDto(business.BusinessId, business.UserId, business.ExpirationDate, "Business_Renewed"));

        return new ApiResponse<int, Exception>(0);
    }
}