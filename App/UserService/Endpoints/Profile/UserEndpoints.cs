using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Profile;
using UserService.Repositories.Profile;
using UserService.Services.Profile;
using UserService.Validators;

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
        services.AddScoped<IUsersService, UsersService>();
    }

    private async Task<IResult> RegisterUser([FromServices] IUsersService usersService, [FromBody] UserDto userData)
    {
        var validator = new UserDtoValidator();
        var results = validator.Validate(userData);
        if (!results.IsValid)
            return Results.BadRequest(results.Errors);

        var result = await usersService.RegisterUser(userData);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> UpdateUser([FromServices] IUsersService usersService, [FromBody] UserDto userData)
    {
        var validator = new UserDtoValidator();
        var results = validator.Validate(userData);
        if (!results.IsValid)
            return Results.BadRequest(results.Errors);

        var result = await usersService.UpdateUser(userData);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> GetUser([FromServices] IUserRepo userRepo)
    {
        var result = await userRepo.GetUser();
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }
}
