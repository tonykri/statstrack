using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Profile;
using UserService.Repositories.Profile;

namespace UserService.Endpoints.Profile;

public class PersonalLifeEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("profile/personallife", RegisterPersonalLife)
            .RequireAuthorization("professional_life_profile_stage");
        app.MapPut("profile/personallife", UpdatePersonalLife)
            .RequireAuthorization("completed_profile");
        app.MapGet("profile/personallife", GetPersonalLife)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IPersonalLifeRepo, PersonalLifeRepo>();
    }

    private IResult RegisterPersonalLife([FromServices] IPersonalLifeRepo personalLifeRepo, [FromBody] PersonalLifeDto userData)
    {
        try
        {
            string token = personalLifeRepo.RegisterPersonalLife(userData);
            return Results.Ok(token);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult UpdatePersonalLife([FromServices] IPersonalLifeRepo personalLifeRepo, [FromBody] PersonalLifeDto userData)
    {
        try
        {
            personalLifeRepo.UpdatePersonalLife(userData);
            return Results.Ok();
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult GetPersonalLife([FromServices] IPersonalLifeRepo personalLifeRepo)
    {
        try
        {
            var personallife = personalLifeRepo.GetPersonalLife();
            return Results.Ok(personallife);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
