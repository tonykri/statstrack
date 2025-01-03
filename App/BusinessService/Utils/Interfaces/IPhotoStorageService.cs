using BusinessService.Dto;

namespace BusinessService.Utils;

public interface IPhotoStorageService
{
    Task<string> UploadPhotoAsync(string fileName, IFormFile file);
    Task<ImageDto> GetPhotoAsync(string fileName);
    Task DeletePhotoAsync(string fileName);
    Task<string> UpdatePhotoAsync(string fileName, IFormFile newFile);
}