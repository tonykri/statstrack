using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Dto;
using ReviewService.Services;

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
        services.AddScoped<IResponseService, ResponseService>();
    }

    public async Task<IResult> PostResponse([FromServices] IResponseService responseService, [FromBody] CreateUpdateResponseDto response)
    {
        var result = await responseService.PostResponse(response);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception =>
            {
                if (exception?.Message == ExceptionMessages.RESPONSE_EXISTS)
                    return Results.BadRequest(exception?.Message);
                else if (exception?.Message == ExceptionMessages.UNAUTHORIZED)
                    return Results.Forbid();
                else
                    return Results.NotFound(exception?.Message);
            }
        );
    }

    public async Task<IResult> UpdateResponse([FromServices] IResponseService responseService, [FromBody] CreateUpdateResponseDto response)
    {
        var result = await responseService.UpdateResponse(response);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception =>
            {
                if (exception?.Message == ExceptionMessages.UNAUTHORIZED)
                    return Results.Forbid();
                else
                    return Results.NotFound(exception?.Message);
            }
        );
    }

    public async Task<IResult> DeleteResponse([FromServices] IResponseService responseService, [FromRoute] Guid reviewId)
    {
        var result = await responseService.DeleteResponse(reviewId);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception =>
            {
                if (exception?.Message == ExceptionMessages.UNAUTHORIZED)
                    return Results.Forbid();
                else
                    return Results.NotFound(exception?.Message);
            }
        );
    }
}