using BusinessService.Dto;
using BusinessService.Models;
using BusinessService.Utils;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Services;

public class BusinessPhotoService : IBusinessPhotoService
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    private readonly IPhotoValidator photoValidator;
    private readonly IBlobStorageService blobStorageService;

    public BusinessPhotoService(DataContext dataContext, ITokenDecoder tokenDecoder, IPhotoValidator photoValidator, IBlobStorageService blobStorageService)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
        this.photoValidator = photoValidator;
        this.blobStorageService = blobStorageService;
    }


    private async void AddPhotos(Business business, IFormFileCollection photos)
    {
        Photo newPhoto;
        foreach (IFormFile photo in photos)
        {
            newPhoto = new Photo
            {
                Business = business,
                BusinessId = business.Id
            };
            await blobStorageService.UploadBlobAsync(newPhoto.PhotoId.ToString(), photo);
        }
    }

    public async Task<ApiResponse<int, Exception>> DeletePhoto(Guid photoId)
    {
        var photo = await dataContext.Photos.FirstOrDefaultAsync(p => p.PhotoId == photoId);
        if (photo is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        await blobStorageService.DeleteBlobAsync(photo.PhotoId.ToString());
        return new ApiResponse<int, Exception>(0);
    }

    public async Task<ApiResponse<int, Exception>> UploadPhotos(Guid businessId, IFormFileCollection photos)
    {
        Guid userId = tokenDecoder.GetUserId();
        if (!dataContext.Businesses.Any(b => b.UserId == userId && b.Id == businessId))
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        var business = await dataContext.Businesses.FirstOrDefaultAsync(b => b.Id == businessId);
        if (business is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_FOUND));

        foreach (IFormFile photo in photos)
        {
            photoValidator.PhotoValidation(photo);
        }

        if (photos.Count() + dataContext.Photos.Where(p => p.BusinessId == businessId).ToList().Count() > 10)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.NOT_VALID));

        AddPhotos(business, photos);
        return new ApiResponse<int, Exception>(0);
    }
}