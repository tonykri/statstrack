using AccountService.Services;
using Config.Stracture;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Endpoints;

public class GoogleAuthEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("signin-google", SignInGoogle);
        app.MapGet("signin-google-callback", SignInGoogleCallback);
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
    }

    private IResult SignInGoogle([FromServices] IGoogleAuthService googleAuthService)
    {
        string url = googleAuthService.SignInGoogle();
        return Results.Redirect(url);
    }

    private async Task<IResult> SignInGoogleCallback([FromServices] IGoogleAuthService googleAuthService, string code, string state)
    {
        var result = await googleAuthService.SignInGoogleCallback(code, state);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }
}
