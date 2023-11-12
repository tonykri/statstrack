using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Profile;
using UserService.Repositories.Profile;

namespace UserService.Endpoints.Profile;

public class ProfilePhotoEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("profile/photo", UploadPhoto)
            .RequireAuthorization("completed_profile");
        app.MapDelete("profile/photo", DeletePhoto)
            .RequireAuthorization("completed_profile");
        app.MapPut("profile/photo", UpdatePhoto)
            .RequireAuthorization("completed_profile");
        app.MapGet("profile/photo", GetPhoto)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IProfilePhotoRepo, ProfilePhotoRepo>();
    }

    private IResult UploadPhoto([FromServices] IProfilePhotoRepo profilePhotoRepo, [FromForm] IFormFile photo)
    {
        try
        {
            ImageDto profilePhoto = profilePhotoRepo.UploadPhoto(photo);
            return Results.File(profilePhoto.PhotoData, profilePhoto.ContentType);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult DeletePhoto([FromServices] IProfilePhotoRepo profilePhotoRepo)
    {
        try
        {
            profilePhotoRepo.DeletePhoto();
            return Results.Ok();
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult UpdatePhoto([FromServices] IProfilePhotoRepo profilePhotoRepo, [FromForm] IFormFile photo)
    {
        try
        {
            ImageDto profilePhoto = profilePhotoRepo.UpdatePhoto(photo);
            return Results.File(profilePhoto.PhotoData, profilePhoto.ContentType);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult GetPhoto([FromServices] IProfilePhotoRepo profilePhotoRepo)
    {
        try
        {
            ImageDto photo = profilePhotoRepo.GetPhoto();
            return Results.File(photo.PhotoData, photo.ContentType);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
