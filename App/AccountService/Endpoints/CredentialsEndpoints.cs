using AccountService.Dto.Request;
using AccountService.Services;
using Config.Stracture;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Endpoints;

public class CredentialsEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("login/{email}", LoginRequest);
        app.MapPost("login", Login);
        app.MapPost("register", Register);
        app.MapGet("delete", DeleteRequest)
            .RequireAuthorization("completed_profile");
        app.MapGet("delete/{code}", Delete)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<ICredentialsService, CredentialsService>();
    }

    private async Task<IResult> LoginRequest([FromServices] ICredentialsService credentialsService, [FromRoute] string email)
    {
        var result = await credentialsService.LoginRequest(email);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => Results.NotFound(exception?.Message)
        );
    }

    private async Task<IResult> Login([FromServices] ICredentialsService credentialsService, [FromBody] LoginDto accountData)
    {
        var result = await credentialsService.Login(accountData);
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

    private async Task<IResult> Register([FromServices] ICredentialsService credentialsService, [FromBody] RegisterDto accountData)
    {
        var result = await credentialsService.Register(accountData);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> DeleteRequest([FromServices] ICredentialsService credentialsService)
    {
        await credentialsService.DeleteRequest();
        return Results.NoContent();
    }
    
    private async Task<IResult> Delete([FromServices] ICredentialsService credentialsService, [FromRoute] string code)
    {
        var result = await credentialsService.Delete(code);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => Results.BadRequest(exception?.Message)
        );
    }
}