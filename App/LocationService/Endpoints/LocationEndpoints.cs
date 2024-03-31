using Config.Stracture;
using LocationService.Dto;
using LocationService.Repositories;
using LocationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocationService.Endpoints;

public class LocationEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("", PostLocation)
            .RequireAuthorization("completed_profile");
        app.MapGet("", GetUsersId)
            .RequireAuthorization("statistics_service");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<ILocationsService, LocationsService>();
        services.AddScoped<ILocationsRepo, LocationsRepo>();
    }

    public async Task<IResult> PostLocation([FromServices] ILocationsService locationsService, [FromBody] LocationDto location)
    {
        var result = await locationsService.PostLocation(location);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> GetUsersId([FromServices] ILocationsRepo locationsRepo, [FromQuery] double businessLat, [FromQuery] double businessLong, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        var ids = await locationsRepo.GetUsersId(businessLat, businessLong, startTime, endTime);
        return Results.Ok(ids);
    }
}