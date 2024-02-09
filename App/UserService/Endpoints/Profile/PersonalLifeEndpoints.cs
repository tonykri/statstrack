using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Profile;
using UserService.Repositories.Profile;
using UserService.Services.Profile;

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
        services.AddScoped<IPersonalLifeService, PersonalLifeService>();
    }

    private async Task<IResult> RegisterPersonalLife([FromServices] IPersonalLifeService personalLifeService, [FromBody] PersonalLifeDto userData)
    {
        var result = await personalLifeService.RegisterPersonalLife(userData);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> UpdatePersonalLife([FromServices] IPersonalLifeService personalLifeService, [FromBody] PersonalLifeDto userData)
    {
        var result = await personalLifeService.UpdatePersonalLife(userData);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> GetPersonalLife([FromServices] IPersonalLifeRepo personalLifeRepo)
    {
        var personallife = await personalLifeRepo.GetPersonalLife();
        return Results.Ok(personallife);
    }
}
