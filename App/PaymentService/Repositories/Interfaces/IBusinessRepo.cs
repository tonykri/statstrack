
namespace PaymentService.Repositories;

public interface IBusinessRepo
{
    void CreateBusiness(Guid user_id, string session_id);
    void RenewLicense(Guid? business_id, string session_id);
}