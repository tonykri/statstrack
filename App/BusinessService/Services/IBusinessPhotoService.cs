using BusinessService.Dto;

namespace BusinessService.Services;

public interface IBusinessPhotoService {
    Task<ApiResponse<int, Exception>> DeletePhoto(Guid photoId);
    Task<ApiResponse<int, Exception>> UploadPhotos(Guid businessId, IFormFileCollection photos);
}