using BusinessService.Dto;
using BusinessService.Models;
using BusinessService.Utils;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Repositories;

public class BusinessPhotosRepo : IBusinessPhotosRepo
{
    private readonly DataContext dataContext;
    private readonly IPhotoValidator photoValidator;

    public BusinessPhotosRepo(DataContext dataContext, IPhotoValidator photoValidator)
    {
        this.dataContext = dataContext;
        this.photoValidator = photoValidator;
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
        string filePath = await dataContext.Photos.Where(p => p.PhotoId == photoId).Select(p => p.PhotoUri).FirstAsync();
        try
        {
            if (File.Exists(filePath))
            {
                byte[] photoData = File.ReadAllBytes(filePath);
                string contentType = photoValidator.GetContentType(filePath);

                var img = new ImageDto(photoData, contentType);
                return new ApiResponse<ImageDto, Exception>(img);
            }
            throw new Exception(ExceptionMessages.NOT_FOUND);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<ImageDto, Exception>(ex);
        }
    }
}