using BusinessService.Dto;
using BusinessService.Models;
using BusinessService.Utils;

namespace BusinessService.Repositories;

public class BusinessPhotosRepo : IBusinessPhotosRepo
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    private readonly IPhotoValidator photoValidator;
    private readonly string folderPath = "/var";

    public BusinessPhotosRepo(DataContext dataContext, ITokenDecoder tokenDecoder, IPhotoValidator photoValidator)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
        this.photoValidator = photoValidator;
    }


    private void AddPhotos(Business business, List<IFormFile> photos)
    {
        Photo newPhoto;
        foreach (IFormFile photo in photos)
        {
            newPhoto = new Photo
            {
                Business = business,
                BusinessId = business.Id
            };
            newPhoto.PhotoUri = Path.Combine(folderPath, newPhoto.PhotoId.ToString() + Path.GetExtension(photo.FileName)); ;

            try
            {
                using (var fileStream = new FileStream(newPhoto.PhotoUri, FileMode.Create))                    
                {
                    photo.CopyTo(fileStream);
                }
                dataContext.Add(photo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
    public List<Guid> GetPhotos(Guid businessId)
    {
        if(!dataContext.Businesses.Any(b => b.Id == businessId))
            throw new NotFoundException("Business not found");
        return dataContext.Photos.Where(p => p.BusinessId == businessId)
            .Select(p => p.PhotoId).ToList();
    }

    public ImageDto GetPhoto(Guid photoId)
    {
        string filePath = dataContext.Photos.Where(p => p.PhotoId == photoId).Select(p => p.PhotoUri).First();
        try
        {
            if (File.Exists(filePath))
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

    public void DeletePhoto(Guid photoId)
    {
        var photo = dataContext.Photos.FirstOrDefault(p => p.PhotoId == photoId);
        if (photo is null)
            return;
        try
        {
            if (File.Exists(photo.PhotoUri))
            {
                File.Delete(photo.PhotoUri);
                dataContext.Remove(photo);
                dataContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void UploadPhotos(Guid businessId, List<IFormFile> photos)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            if(!dataContext.Businesses.Any(b => b.UserId == userId&& b.Id == businessId))
                throw new NotFoundException("There is no business that matches the given user");
            var business = dataContext.Businesses.FirstOrDefault(b => b.Id == businessId);
            if (business is null)
                throw new NotFoundException("Business not found");

            foreach (IFormFile photo in photos)
            {
                photoValidator.PhotoValidation(photo);
            }

            if (photos.Count() + dataContext.Photos.Where(p => p.BusinessId == businessId).ToList().Count() > 10)
                throw new NotValidException("Cannot have more than 10 photos");

            AddPhotos(business, photos);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}