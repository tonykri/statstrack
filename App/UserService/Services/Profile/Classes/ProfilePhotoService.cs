using Microsoft.EntityFrameworkCore;
using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Profile;

public class ProfilePhotoService : IProfilePhotoService
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly IPhotoValidator photoValidator;
    private readonly IBlobStorageService blobStorageService;
    public ProfilePhotoService(ITokenDecoder tokenDecoder, DataContext dataContext, IPhotoValidator photoValidator, IBlobStorageService blobStorageService)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.photoValidator = photoValidator;
        this.blobStorageService = blobStorageService;
    }

    public async Task<ApiResponse<int, Exception>> UploadPhoto(IFormFile photo)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            photoValidator.PhotoValidation(photo);
            var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
            if (user.PhotoUrl is not null)
                return new ApiResponse<int, Exception>(new Exception("Photo exists"));

            string fileName = Guid.NewGuid().ToString();
            await blobStorageService.UploadBlobAsync(fileName, photo);

            user.PhotoUrl = fileName;
            await dataContext.SaveChangesAsync();

            return new ApiResponse<int, Exception>(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<int, Exception>(new Exception());
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
            string? fileName = user.PhotoUrl;

            if (user.PhotoUrl is not null)
                await blobStorageService.DeleteBlobAsync(user.PhotoUrl);
            user.PhotoUrl = null;
            await dataContext.SaveChangesAsync();

            return new ApiResponse<int, Exception>(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<int, Exception>(new Exception());
        }
    }

    public async Task<ApiResponse<int, Exception>> UpdatePhoto(IFormFile photo)
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
            return new ApiResponse<int, Exception>(new Exception());
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
            string? fileName = user.PhotoUrl;

            if (fileName is not null)
            {
                var img = await blobStorageService.GetBlobAsync(fileName);
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