using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Profile;
using UserService.Repositories.Profile;

namespace UserService.Endpoints.Profile;

public class UserEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("profile/user", RegisterUser)
            .RequireAuthorization("user_basics_profile_stage");
        app.MapPut("profile/user", UpdateUser)
            .RequireAuthorization("completed_profile");
        app.MapGet("profile/user", GetUser)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IUserRepo, UserRepo>();
    }

    private IResult RegisterUser([FromServices] IUserRepo userRepo, [FromBody] UserDto userData)
    {
        try
        {
            string token = userRepo.RegisterUser(userData);
            return Results.Ok(token);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult UpdateUser([FromServices] IUserRepo userRepo, [FromBody] UserDto userData)
    {
        try
        {
            userRepo.UpdateUser(userData);
            return Results.Ok();
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult GetUser([FromServices] IUserRepo userRepo)
    {
        try
        {
            var user = userRepo.GetUser();
            return Results.Ok(user);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
