
using PaymentService.AsymcDataProcessing.MessageBusClient;
using PaymentService.Dto.MessageBus.Send;
using PaymentService.Models;

namespace PaymentService.Repositories;

public class BusinessRepo : IBusinessRepo
{
    private readonly DataContext dataContext;
    private readonly IMessageBusClient messageBusClient;
    public BusinessRepo(DataContext dataContext, IMessageBusClient messageBusClient)
    {
        this.dataContext = dataContext;
        this.messageBusClient = messageBusClient;
    }
    public void CreateBusiness(Guid user_id, string session_id)
    {
        if(dataContext.Payments.Any(p => p.Id.Equals(session_id)))
            throw new PaymentExistsException("Payment with current id already exists");
            
        Business business = new Business
        {
            UserId = user_id,
            ExpirationDate = DateTime.Now.AddYears(1)
        };
        Payment payment = new Payment
        {
            Id = session_id,
            Business = business,
            BusinessId = business.BusinessId
        };

        dataContext.Add(business);
        dataContext.Add(payment);
        dataContext.SaveChanges();
        messageBusClient.BusinessCreateRenew(new BusinessCreatedRenewedDto(business.BusinessId, business.UserId, business.ExpirationDate, "Business_Created"));
    }

    public void RenewLicense(Guid? business_id, string session_id)
    {
        if(dataContext.Payments.Any(p => p.Id.Equals(session_id)))
            throw new PaymentExistsException("Payment with current id already exists");

        var business = dataContext.Businesses.FirstOrDefault(b => b.BusinessId == business_id);
        if(business is null)
            throw new NotFoundException("Business not found");
        
        Payment payment = new Payment
        {
            Id = session_id,
            Business = business,
            BusinessId = business.BusinessId
        };
        business.ExpirationDate = business.ExpirationDate.AddYears(1);

        dataContext.Add(payment);
        dataContext.SaveChanges();
        messageBusClient.BusinessCreateRenew(new BusinessCreatedRenewedDto(business.BusinessId, business.UserId, business.ExpirationDate, "Business_Renewed"));

    }
}