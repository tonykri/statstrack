using BusinessService.Dto;
using BusinessService.Models;
using BusinessService.Repositories;
using BusinessService.Services;
using BusinessService.Utils;
using Config.Stracture;
using Microsoft.AspNetCore.Mvc;

namespace BusinessService.Endpoints;

public class BusinessEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPut("update", UpdateBusiness)
            .RequireAuthorization("completed_profile");
        app.MapGet("{businessId:guid}", GetBusiness)
            .RequireAuthorization("completed_profile");
        app.MapGet("mybusinesses", GetMyBusinesses)
            .RequireAuthorization("completed_profile");
        app.MapGet("businesses", GetBusinesses)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IBusinessRepo, BusinessRepo>();
        services.AddScoped<IBusinessesService, BusinessesService>();
    }

    private async Task<IResult> UpdateBusiness([FromServices] IBusinessesService businessesService, [FromBody] BusinessDto business)
    {
        var validator = new BusinessDtoValidator();
        var results = validator.Validate(business);
        if(!results.IsValid)
            return Results.BadRequest(results.Errors);

        var result = await businessesService.UpdateBusiness(business);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => {
                if (exception?.Message == ExceptionMessages.NOT_FOUND)
                    return Results.NotFound(exception?.Message);
                else if (exception?.Message == ExceptionMessages.UNAUTHORIZED)
                    return Results.Unauthorized();
                else
                    return Results.BadRequest(exception?.Message);
            }
        );
    }

    private async Task<IResult> GetBusiness([FromServices] IBusinessRepo businessRepo, [FromRoute] Guid businessId)
    {
        var result = await businessRepo.GetBusiness(businessId);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.NotFound(exception?.Message)
        );
    }

    private async Task<IResult> GetMyBusinesses([FromServices] IBusinessRepo businessRepo)
    {
        var result = await businessRepo.GetMyBusinesses();
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.NotFound(exception?.Message)
        );
    }

    private async Task<IResult> GetBusinesses([FromServices] IBusinessRepo businessRepo, [FromQuery]double upperLat, [FromQuery]double upperLong, [FromQuery]double bottomLat, [FromQuery]double bottomLong)
    {
        var result = await businessRepo.GetBusinesses(upperLat, upperLong, bottomLat, bottomLong);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.NotFound(exception?.Message)
        );
    }
}