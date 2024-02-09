using UserService.Dto;
using UserService.Dto.Profile;

namespace UserService.Repositories.Profile;

public interface IProfilePhotoRepo
{
    Task<ApiResponse<ImageDto, Exception>> GetPhoto();
}