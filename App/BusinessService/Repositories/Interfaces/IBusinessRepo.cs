using BusinessService.Dto;
using BusinessService.Models;

namespace BusinessService.Repositories;

public interface IBusinessRepo
{
    void UpdateBusiness(BusinessDto business);
    Business GetBusiness(Guid businessId);
    List<Business> GetMyBusinesses();
    List<Business> GetBusinesses(double upperLat, double upperLong, double bottomLat, double bottomLong);
}