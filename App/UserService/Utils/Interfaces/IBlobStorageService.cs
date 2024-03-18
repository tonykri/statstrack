using UserService.Dto.Profile;

namespace UserService.Utils;

public interface IBlobStorageService
{
    Task UploadBlobAsync(string blobName, IFormFile file);
    Task<ImageDto> GetBlobAsync(string blobName);
    Task DeleteBlobAsync(string blobName);
    Task UpdateBlobAsync(string blobName, IFormFile file);
}