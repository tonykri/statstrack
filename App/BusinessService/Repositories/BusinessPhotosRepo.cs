using BusinessService.Dto;
using BusinessService.Models;
using BusinessService.Utils;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Repositories;

public class BusinessPhotosRepo : IBusinessPhotosRepo
{
    private readonly DataContext dataContext;
    private readonly IBlobStorageService blobStorageService;
    private readonly IPhotoStorageService photoStorageService;

    public BusinessPhotosRepo(DataContext dataContext, IBlobStorageService blobStorageService, IPhotoStorageService photoStorageService)
    {
        this.dataContext = dataContext;
        this.blobStorageService = blobStorageService;
        this.photoStorageService = photoStorageService;
    }

    public async Task<ApiResponse<List<Guid>, Exception>> GetPhotos(Guid businessId)
    {
        if (!dataContext.Businesses.Any(b => b.Id == businessId))
            return new ApiResponse<List<Guid>, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
        var data = await dataContext.Photos.Where(p => p.BusinessId == businessId)
            .Select(p => p.PhotoId).ToListAsync();
        return new ApiResponse<List<Guid>, Exception>(data);
    }

    public async Task<ApiResponse<ImageDto, Exception>> GetPhoto(Guid photoId)
    {
        var fileName = await dataContext.Photos.Where(p => p.PhotoId == photoId).Select(p => p.PhotoId).FirstAsync();
        if (fileName.ToString() is not null)
        {
            //var img = await blobStorageService.GetBlobAsync(fileName.ToString());
            var img = await photoStorageService.GetPhotoAsync(fileName.ToString());
            return new ApiResponse<ImageDto, Exception>(img);
        }
        return new ApiResponse<ImageDto, Exception>(new Exception(ExceptionMessages.NOT_FOUND));
    }
}