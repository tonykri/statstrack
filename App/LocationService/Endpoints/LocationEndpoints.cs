using Config.Stracture;
using LocationService.Dto;
using LocationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocationService.Endpoints;

public class LocationEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("", PostLocation)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<ILocationsService, LocationsService>();
    }

    public async Task<IResult> PostLocation([FromServices] ILocationsService locationsService, [FromBody] LocationDto location)
    {
        var result = await locationsService.PostLocation(location);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => Results.BadRequest(exception?.Message)
        );
    }
}