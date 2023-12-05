using BusinessService.Dto;

namespace BusinessService.Repositories;

public interface IBusinessPhotosRepo
{
    List<Guid> GetPhotos(Guid businessId);
    ImageDto GetPhoto(Guid photoId);
    void DeletePhoto(Guid photoId);
    void UploadPhotos(Guid businessId, IFormFileCollection photos);
}