using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Dto;
using ReviewService.Repositories;

namespace ReviewService.Endpoints;

public class ResponseEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("response", PostResponse)
            .RequireAuthorization("completed_profile");
        app.MapPut("response", UpdateResponse)
            .RequireAuthorization("completed_profile");
        app.MapDelete("response/{reviewId:guid}", DeleteResponse)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IResponseRepo, ResponseRepo>();
    }

    public IResult PostResponse([FromServices] IResponseRepo responseRepo, [FromBody] CreateUpdateResponseDto response)
    {
        try
        {
            responseRepo.PostResponse(response);
            return Results.NoContent();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(ReviewResExistsException ex)
        {
            return Results.BadRequest(ex.Message);
        }catch(NotAllowedException ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Forbid();
        }
    }

    public IResult UpdateResponse([FromServices] IResponseRepo responseRepo, [FromBody] CreateUpdateResponseDto response)
    {
        try
        {
            responseRepo.UpdateResponse(response);
            return Results.NoContent();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(NotAllowedException ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Forbid();
        }
    }

    public IResult DeleteResponse([FromServices] IResponseRepo responseRepo, [FromRoute] Guid reviewId)
    {
        try
        {
            responseRepo.DeleteResponse(reviewId);
            return Results.NoContent();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(NotAllowedException ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Forbid();
        }
    }
}