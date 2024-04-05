using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using StatisticsService.Repositories;

namespace StatisticsService.Endpoints;

public class StatisticsEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("businessstats", GetBusinessStats)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IStatsRepo, StatsRepo>();
    }

    private async Task<IResult> GetBusinessStats([FromServices] IStatsRepo statsRepo, [FromQuery] Guid businessId, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        var result = await statsRepo.GetBusinessStats(businessId, startTime, endTime);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception =>
            {
                if (exception is null)
                    return Results.BadRequest();
                if (exception.Message.Equals(ExceptionMessages.BUSINESS_NOT_FOUND))
                    return Results.NotFound(exception.Message);
                else if (exception.Message.Equals(ExceptionMessages.UNAUTHORIZED))
                    return Results.Unauthorized();
                else
                    return Results.BadRequest(exception.Message);
            }
        );
    }
}