using UserService.Dto;
using UserService.Dto.Profile;

namespace UserService.Services.Profile;

public interface IProfilePhotoService
{
    Task<ApiResponse<int, Exception>> UploadPhoto(IFormFile photo);
    Task<ApiResponse<int, Exception>> DeletePhoto();
    Task<ApiResponse<int, Exception>> UpdatePhoto(IFormFile photo);
    Task<ApiResponse<ImageDto, Exception>> GetPhoto();
}