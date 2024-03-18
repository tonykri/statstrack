using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Account;
using UserService.Services.Account;

namespace UserService.Endpoints.Account;

public class LoginCredentialsEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("account/login/{email}", SendLoginEmail);
        app.MapPost("account/login/credentials", LoginCredentials);
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<ILoginCredentialsService, LoginCredentialsService>();
    }

    private async Task<IResult> SendLoginEmail([FromServices] ILoginCredentialsService loginCredentialsService, [FromRoute] string email)
    {
        var result = await loginCredentialsService.SendLoginEmail(email);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => {
                if (exception?.Message == ExceptionMessages.NOT_FOUND)
                    return Results.NotFound(exception?.Message);
                else
                    return Results.BadRequest(exception?.Message);
            }
        );
    }

    private async Task<IResult> LoginCredentials([FromServices] ILoginCredentialsService loginCredentialsService, [FromBody] LoginCredentialsDto userData)
    {
        var result = await loginCredentialsService.LoginCredentials(userData);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => {
                if (exception?.Message == ExceptionMessages.NOT_FOUND)
                    return Results.NotFound(exception?.Message);
                else
                    return Results.BadRequest(exception?.Message);
            }
        );
    }

}
