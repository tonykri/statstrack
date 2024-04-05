using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto;
using UserService.Repositories;

namespace UserService.Endpoints;

public class StatsServiceEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("stats", GetBusinessStats)
            .RequireAuthorization("statistics_service");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IStatsServiceRepo, StatsServiceRepo>();
    }

    private IResult GetBusinessStats([FromServices] IStatsServiceRepo statsServiceRepo, [FromBody] UserIdsDto userIdsDto)
    {
        var profiles = statsServiceRepo.GetBusinessStats(userIdsDto);
        return Results.Ok(profiles);
    }
}