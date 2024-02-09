using BusinessService.Dto;
using BusinessService.Models;

namespace BusinessService.Repositories;

public interface IBusinessRepo
{
    Task<ApiResponse<Business, Exception>> GetBusiness(Guid businessId);
    Task<ApiResponse<List<Business>, Exception>> GetMyBusinesses();
    Task<ApiResponse<List<Business>, Exception>> GetBusinesses(double upperLat, double upperLong, double bottomLat, double bottomLong);
}