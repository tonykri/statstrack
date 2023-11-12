using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Profile;
using UserService.Repositories.Profile;

namespace UserService.Endpoints.Profile;

public class HobbiesEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("profile/hobbies", RegisterHobbies)
            .RequireAuthorization("personal_life_profile_stage");
        app.MapPut("profile/hobbies", UpdateHobbies)
            .RequireAuthorization("completed_profile");
        app.MapGet("profile/hobbies", GetHobbies)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IHobbiesRepo, HobbiesRepo>();
    }

    private IResult RegisterHobbies([FromServices] IHobbiesRepo hobbiesRepo, [FromBody] HobbiesDto userData)
    {
        try
        {
            string token = hobbiesRepo.RegisterHobbies(userData);
            return Results.Ok(token);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult UpdateHobbies([FromServices] IHobbiesRepo hobbiesRepo, [FromBody] HobbiesDto userData)
    {
        try
        {
            hobbiesRepo.UpdateHobbies(userData);
            return Results.Ok();
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult GetHobbies([FromServices] IHobbiesRepo hobbiesRepo)
    {
        try
        {
            var expenses = hobbiesRepo.GetHobbies();
            return Results.Ok(expenses);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
