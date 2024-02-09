namespace UserService.Utils;

public class PhotoValidator : IPhotoValidator
{
    public string GetContentType(string fileName)
    {
        string extension = Path.GetExtension(fileName);
        switch (extension.ToLower())
        {
            case ".jpeg":
            case ".jpg":
                return "image/jpeg";
            case ".png":
                return "image/png";
        }
        return "";
    }
    public bool IsImageFile(string contentType)
    {
        List<string> allowedTypes = new List<string>
        {
            "image/jpeg",
            "image/jpg",
            "image/png"
        };
        return allowedTypes.Contains(contentType);
    }
    public void PhotoValidation(IFormFile photo)
    {
        if (photo is null || photo.Length == 0)
            throw new Exception("Invalid file");
        if (photo.Length > 5242880)
            throw new Exception("File size too large. Maximum file size allowed is 5 MB");
        if (!IsImageFile(photo.ContentType))
            throw new Exception("Invalid file type");
    }
}