using BusinessService.Dto;
using BusinessService.Repositories;
using BusinessService.Services;
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
            .RequireAuthorization("completed_profile")
            .DisableAntiforgery(); // For now, disable antiforgery to test photo upload
        app.MapDelete("photos/{photoId:guid}", DeletePhoto)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IBusinessPhotosRepo, BusinessPhotosRepo>();
        services.AddScoped<IBusinessPhotoService, BusinessPhotoService>();
    }

    private async Task<IResult> GetPhotos([FromServices] IBusinessPhotosRepo businessPhotosRepo, [FromRoute] Guid businessId)
    {
        var result = await businessPhotosRepo.GetPhotos(businessId);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.NotFound(exception?.Message)
        );
    }

    private async Task<IResult> GetPhoto([FromServices] IBusinessPhotosRepo businessPhotosRepo, [FromRoute] Guid businessId, [FromRoute] Guid photoId)
    {
        var result = await businessPhotosRepo.GetPhoto(photoId);
        return result.Match<IResult>(
            data =>
            {
                if (data is null)
                    return Results.BadRequest();
                else
                    return Results.File(data.PhotoData, data.ContentType);
            },
            exception =>
            {
                if (exception?.Message == ExceptionMessages.NOT_FOUND)
                    return Results.NotFound(exception?.Message);
                else
                    return Results.BadRequest(exception?.Message);
            }
        );
    }

    private async Task<IResult> UploadPhotos([FromServices] IBusinessPhotoService businessPhotoService, [FromRoute] Guid businessId, HttpRequest request)
    {
        request.EnableBuffering();
        var photos = request.Form.Files;
        var result = await businessPhotoService.UploadPhotos(businessId, photos);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => exception?.Message == ExceptionMessages.NOT_FOUND 
                ? Results.NotFound(exception?.Message)
                : Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> DeletePhoto([FromServices] IBusinessPhotoService businessPhotoService, [FromRoute] Guid photoId)
    {
        var result = await businessPhotoService.DeletePhoto(photoId);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => Results.BadRequest(exception?.Message)
        );
    }
}