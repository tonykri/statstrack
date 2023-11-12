using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Account;
using UserService.Repositories.Account;

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
        services.AddScoped<IRegisterCredentialsRepo, RegisterCredentialsRepo>();
    }

    public IResult RegisterCredentials([FromServices] IRegisterCredentialsRepo registerCredentialsRepo, [FromBody] RegisterCredentialsDto user)
    {
        try
        {
            string data = registerCredentialsRepo.RegisterCredentials(user);
            return Results.Ok(data);
        }catch(UserExistsException ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Forbid();
        }
    }

    public IResult RegisterEmailVerify([FromServices] IRegisterCredentialsRepo registerCredentialsRepo, [FromRoute] string code)
    {
        try
        {
            string data = registerCredentialsRepo.RegisterEmailVerify(code);
            return Results.Ok(data);
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
    }
}
