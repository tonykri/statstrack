using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Services.Profile;

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
        services.AddScoped<IProfilePhotoService, ProfilePhotoService>();
    }

    private async Task<IResult> UploadPhoto([FromServices] IProfilePhotoService profilePhotoService, [FromForm] IFormFile photo)
    {
        var result = await profilePhotoService.UploadPhoto(photo);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> DeletePhoto([FromServices] IProfilePhotoService profilePhotoService)
    {
        var result = await profilePhotoService.DeletePhoto();
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> UpdatePhoto([FromServices] IProfilePhotoService profilePhotoService, [FromForm] IFormFile photo)
    {
        var result = await profilePhotoService.UpdatePhoto(photo);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> GetPhoto([FromServices] IProfilePhotoService profilePhotoService)
    {
        var result = await profilePhotoService.GetPhoto();
        return result.Match<IResult>(
            data =>
            {
                if (data is null)
                    return Results.BadRequest();
                else
                    return Results.File(data.PhotoData, data.ContentType);
            },
            exception => Results.BadRequest(exception?.Message)
        );
    }
}
