using Microsoft.EntityFrameworkCore;
using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class ProfilePhotoRepo : IProfilePhotoRepo
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly IPhotoValidator photoValidator;
    public ProfilePhotoRepo(ITokenDecoder tokenDecoder, DataContext dataContext, IPhotoValidator photoValidator)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.photoValidator = photoValidator;
    }

    public async Task<ApiResponse<ImageDto, Exception>> GetPhoto()
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

            return new ApiResponse<ImageDto, Exception>(new ImageDto(photoData, contentType));
        }
        return new ApiResponse<ImageDto, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
    }
}