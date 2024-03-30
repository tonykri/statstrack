using AccountService.Services;
using Config.Stracture;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Endpoints;

public class RefreshTokenEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("refreshtoken", RefreshToken);
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
    }

    private async Task<IResult> RefreshToken([FromServices] IRefreshTokenService refreshTokenService, [FromHeader] string token)
    {
        var result = await refreshTokenService.RefreshToken(token);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception =>
            {
                if (exception?.Message == ExceptionMessages.NOT_FOUND)
                    return Results.NotFound(exception?.Message);
                else
                    return Results.BadRequest(exception?.Message);
            }
        );
    }
}