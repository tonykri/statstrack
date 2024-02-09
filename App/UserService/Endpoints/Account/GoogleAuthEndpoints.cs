using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Services.Account;

namespace UserService.Endpoints.Account;

public class GoogleAuthEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("account/signin-google", SignInGoogle);
        app.MapGet("account/signin-google-callback", SignInGoogleCallback);
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
    }

    private IResult SignInGoogle([FromServices] IGoogleAuthService googleAuthService)
    {
        string authorizationUrl = googleAuthService.SignInGoogle();
        return Results.Redirect(authorizationUrl);
    }

    private async Task<IResult> SignInGoogleCallback([FromServices] IGoogleAuthService googleAuthService, string code, string state)
    {
        var data = await googleAuthService.SignInGoogleCallback(code, state);
        return Results.Ok(data);
    }
}
