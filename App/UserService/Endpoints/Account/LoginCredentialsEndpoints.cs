using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Account;
using UserService.Repositories.Account;

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
        services.AddScoped<ILoginCredentialsRepo, LoginCredentialsRepo>();
    }

    private IResult SendLoginEmail([FromServices] ILoginCredentialsRepo loginCredentialsRepo, [FromRoute] string email)
    {
        try
        {
            loginCredentialsRepo.SendLoginEmail(email);
            return Results.NoContent();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult LoginCredentials([FromServices] ILoginCredentialsRepo loginCredentialsRepo, [FromBody] LoginCredentialsDto userData)
    {
        try
        {
            UserDataDto data = loginCredentialsRepo.LoginCredentials(userData);
            return Results.Ok(data);
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
