using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Services.Account;

public class RefreshTokenEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("account/refreshtoken", RefreshToken)
            .RequireAuthorization();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
    }

    private async Task<IResult> RefreshToken([FromServices] IRefreshTokenService refreshTokenService) 
    {
        var result = await refreshTokenService.RefreshToken();
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }
}
