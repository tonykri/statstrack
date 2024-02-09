using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Profile;
using UserService.Repositories.Profile;
using UserService.Services.Profile;
using UserService.Validators;

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
        services.AddScoped<IHobbiesService, HobbiesService>();
    }

    private async Task<IResult> RegisterHobbies([FromServices] IHobbiesService hobbiesService, [FromBody] HobbiesDto userData)
    {
        var validator = new HobbiesDtoValidator();
        var results = validator.Validate(userData);
        if (!results.IsValid)
            return Results.BadRequest(results.Errors);

        var result = await hobbiesService.RegisterHobbies(userData);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> UpdateHobbies([FromServices] IHobbiesService hobbiesService, [FromBody] HobbiesDto userData)
    {
        var validator = new HobbiesDtoValidator();
        var results = validator.Validate(userData);
        if (!results.IsValid)
            return Results.BadRequest(results.Errors);

        var result = await hobbiesService.UpdateHobbies(userData);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> GetHobbies([FromServices] IHobbiesRepo hobbiesRepo)
    {
        var expenses = await hobbiesRepo.GetHobbies();
        return Results.Ok(expenses);
    }
}
