using BusinessService.Repositories;
using Config.Stracture;
using Microsoft.AspNetCore.Mvc;

namespace BusinessService.Endpoints;

public class StatsServiceEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("stats/location", GetBusinessLocation)
            .RequireAuthorization("statistics_service");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IStatsServiceRepo, StatsServiceRepo>();
    }

    private async Task<IResult> GetBusinessLocation([FromServices] IStatsServiceRepo statsServiceRepo, [FromQuery] Guid businessId)
    {
        var result = await statsServiceRepo.GetBusinessLocation(businessId);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }
}