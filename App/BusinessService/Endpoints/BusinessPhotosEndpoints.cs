using BusinessService.Dto;
using BusinessService.Repositories;
using Config.Stracture;
using Microsoft.AspNetCore.Mvc;

namespace BusinessService.Endpoints;

public class BusinessPhotosEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("photos/{businessId:guid}", GetPhotos)
            .RequireAuthorization("completed_profile");
        app.MapGet("photos/{businessId:guid}/{photoId:guid}", GetPhoto)
            .RequireAuthorization("completed_profile");
        app.MapPost("photos/{businessId:guid}", UploadPhotos)
            .RequireAuthorization("completed_profile");
        app.MapDelete("photos/{photoId:guid}", DeletePhoto)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IBusinessPhotosRepo, BusinessPhotosRepo>();
    }

    private IResult GetPhotos([FromServices] IBusinessPhotosRepo businessPhotosRepo, [FromRoute] Guid businessId)
    {
        try
        {
            List<Guid> photoIds = businessPhotosRepo.GetPhotos(businessId);
            return Results.Ok(photoIds);
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
    }

    private IResult GetPhoto([FromServices] IBusinessPhotosRepo businessPhotosRepo, [FromRoute] Guid businessId, [FromRoute] Guid photoId)
    {
        try
        {
            ImageDto photo = businessPhotosRepo.GetPhoto(photoId);
            return Results.File(photo.PhotoData, photo.ContentType);
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult UploadPhotos([FromServices] IBusinessPhotosRepo businessPhotosRepo, [FromRoute] Guid businessId, [FromForm] List<IFormFile> photos)
    {
        try
        {
            businessPhotosRepo.UploadPhotos(businessId, photos);
            return Results.Ok();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult DeletePhoto([FromServices] IBusinessPhotosRepo businessPhotosRepo, [FromRoute] Guid photoId)
    {
        try
        {
            businessPhotosRepo.DeletePhoto(photoId);
            return Results.Ok();
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}