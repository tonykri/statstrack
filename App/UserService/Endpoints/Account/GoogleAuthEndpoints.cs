using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Repositories.Account;

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
        services.AddScoped<IGoogleAuthRepo, GoogleAuthRepo>();
    }

    private IResult SignInGoogle([FromServices] IGoogleAuthRepo googleAuthRepo)
    {
        string authorizationUrl = googleAuthRepo.SignInGoogle();
        return Results.Redirect(authorizationUrl);
    }

    private async Task<IResult> SignInGoogleCallback([FromServices] IGoogleAuthRepo googleAuthRepo, string code, string state)
    {
        var data = await googleAuthRepo.SignInGoogleCallback(code, state);
        return Results.Ok(data);
    }
}
