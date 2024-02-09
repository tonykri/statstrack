using BusinessService.Dto;

namespace BusinessService.Repositories;

public interface IBusinessPhotosRepo
{
    Task<ApiResponse<List<Guid>, Exception>> GetPhotos(Guid businessId);
    Task<ApiResponse<ImageDto, Exception>> GetPhoto(Guid photoId);
}