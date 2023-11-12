using Config.Stracture;
using LocationService.Dto;
using LocationService.Repositories;
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
        services.AddScoped<ILocationRepo, LocationRepo>();
    }

    public IResult PostLocation([FromServices] ILocationRepo locationRepo, [FromBody] LocationDto location)
    {
        try
        {
            locationRepo.PostLocation(location);
            return Results.NoContent();
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}