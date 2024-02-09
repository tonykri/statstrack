using BusinessService.Dto;

namespace BusinessService.Services;

public interface IBusinessesService {
    Task<ApiResponse<int, Exception>> UpdateBusiness(BusinessDto business);
} 