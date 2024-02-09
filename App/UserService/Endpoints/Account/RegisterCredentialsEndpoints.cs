using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Account;
using UserService.Services.Account;

namespace UserService.Endpoints.Account;

public class RegisterCredentialsEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("account/register/credentials", RegisterCredentials);
        app.MapGet("account/register/verifyemail/{code}", RegisterEmailVerify)
            .RequireAuthorization("email_confirmation_profile_stage");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRegisterCredentialsService, RegisterCredentialsService>();
    }

    public async Task<IResult> RegisterCredentials([FromServices] IRegisterCredentialsService registerCredentialsService, [FromBody] RegisterCredentialsDto user)
    {
        var result = await registerCredentialsService.RegisterCredentials(user);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    public async Task<IResult> RegisterEmailVerify([FromServices] IRegisterCredentialsService registerCredentialsService, [FromRoute] string code)
    {
        var result = await registerCredentialsService.RegisterEmailVerify(code);
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
