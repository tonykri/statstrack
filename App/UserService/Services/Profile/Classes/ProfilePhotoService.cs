using Microsoft.EntityFrameworkCore;
using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Profile;

public class ProfilePhotoService : IProfilePhotoService
{
    private readonly string folderPath = "/var";
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly IPhotoValidator photoValidator;
    public ProfilePhotoService(ITokenDecoder tokenDecoder, DataContext dataContext, IPhotoValidator photoValidator)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.photoValidator = photoValidator;
    }

    public async Task<ApiResponse<ImageDto, Exception>> UploadPhoto(IFormFile photo)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            photoValidator.PhotoValidation(photo);
            var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                return new ApiResponse<ImageDto, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
            if (user.PhotoUrl is not null)
                return new ApiResponse<ImageDto, Exception>(new Exception("Photo exists"));

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            string filePath = Path.Combine(folderPath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }
            user.PhotoUrl = filePath;
            await dataContext.SaveChangesAsync();
            return await GetPhoto();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<ImageDto, Exception>(new Exception());
        }
    }

    public async Task<ApiResponse<int, Exception>> DeletePhoto()
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
            string? filePath = user.PhotoUrl;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                user.PhotoUrl = null;
                await dataContext.SaveChangesAsync();
            }
            return new ApiResponse<int, Exception>(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<int, Exception>(new Exception());
        }
    }

    public async Task<ApiResponse<ImageDto, Exception>> UpdatePhoto(IFormFile photo)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            await DeletePhoto();
            return await UploadPhoto(photo);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<ImageDto, Exception>(new Exception());
        }
    }

    public async Task<ApiResponse<ImageDto, Exception>> GetPhoto()
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                return new ApiResponse<ImageDto, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
            string? filePath = user.PhotoUrl;

            if (File.Exists(filePath))
            {
                byte[] photoData = File.ReadAllBytes(filePath);
                string contentType = photoValidator.GetContentType(filePath);

                var img = new ImageDto(photoData, contentType);
                return new ApiResponse<ImageDto, Exception>(img);
            }
            return new ApiResponse<ImageDto, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<ImageDto, Exception>(new Exception());
        }
    }
}