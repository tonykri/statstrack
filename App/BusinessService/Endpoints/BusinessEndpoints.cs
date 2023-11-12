using BusinessService.Dto;
using BusinessService.Models;
using BusinessService.Repositories;
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
    }

    private IResult UpdateBusiness([FromServices] IBusinessRepo businessRepo, [FromBody] BusinessDto business)
    {
        try
        {
            businessRepo.UpdateBusiness(business);
            return Results.NoContent();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(NotAllowedException ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Forbid();
        }catch(NotValidException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult GetBusiness([FromServices] IBusinessRepo businessRepo, [FromRoute] Guid businessId)
    {
        try
        {
            Business data = businessRepo.GetBusiness(businessId);
            return Results.Ok(data);
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
    }

    private IResult GetMyBusinesses([FromServices] IBusinessRepo businessRepo)
    {
        List<Business> data = businessRepo.GetMyBusinesses();
        return Results.Ok(data);
    }

    private IResult GetBusinesses([FromServices] IBusinessRepo businessRepo, [FromQuery]double upperLat, [FromQuery]double upperLong, [FromQuery]double bottomLat, [FromQuery]double bottomLong)
    {
        List<Business> data = businessRepo.GetBusinesses(upperLat, upperLong, bottomLat, bottomLong);
        return Results.Ok(data);
    }
}