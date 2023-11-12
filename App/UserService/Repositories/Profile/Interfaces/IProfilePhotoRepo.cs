using UserService.Dto.Profile;

namespace UserService.Repositories.Profile;

public interface IProfilePhotoRepo
{
    ImageDto UploadPhoto(IFormFile photo);
    void DeletePhoto();
    ImageDto UpdatePhoto(IFormFile photo);
    ImageDto GetPhoto();
}