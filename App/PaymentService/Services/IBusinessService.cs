
using PaymentService.Dto;

namespace PaymentService.Services;

public interface IBusinessService
{
    Task<ApiResponse<int, Exception>> CreateBusiness(Guid user_id, string session_id);
    Task<ApiResponse<int, Exception>> RenewLicense(Guid? business_id, string session_id);
}