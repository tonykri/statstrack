namespace BusinessService.Utils;

public interface IPhotoValidator
{
    string GetContentType(string fileName);
    bool IsImageFile(string contentType);
    void PhotoValidation(IFormFile photo);
}