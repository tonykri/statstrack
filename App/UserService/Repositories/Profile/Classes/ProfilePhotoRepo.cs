using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class ProfilePhotoRepo : IProfilePhotoRepo
{
    private readonly string folderPath = "/var";
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly IPhotoValidator photoValidator;
    public ProfilePhotoRepo(ITokenDecoder tokenDecoder, DataContext dataContext, IPhotoValidator photoValidator)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.photoValidator = photoValidator;
    }

    public ImageDto UploadPhoto(IFormFile photo)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            photoValidator.PhotoValidation(photo);
            var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
            if(user is null)
                throw new NotFoundException("User not found");
            if(user.PhotoUrl is not null)
                throw new ProfilePhotoException("Profile photo exists");
            
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            string filePath = Path.Combine(folderPath, fileName);
            using(var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }
            user.PhotoUrl = filePath;
            dataContext.SaveChanges();
            return GetPhoto();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void DeletePhoto()
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
            if(user is null)
                throw new NotFoundException("User not found");
            string? filePath = user.PhotoUrl;

            if(File.Exists(filePath))
            {
                File.Delete(filePath);
                user.PhotoUrl = null;
                dataContext.SaveChanges();
            }
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public ImageDto UpdatePhoto(IFormFile photo)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            DeletePhoto();
            return UploadPhoto(photo);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public ImageDto GetPhoto()
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
            if(user is null)
                throw new NotFoundException("User not found");
            string? filePath = user.PhotoUrl;

            if(File.Exists(filePath))
            {
                byte[] photoData = File.ReadAllBytes(filePath);
                string contentType = photoValidator.GetContentType(filePath);

                return new ImageDto(photoData, contentType);
            }
            throw new NotFoundException("Photo not found");
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}