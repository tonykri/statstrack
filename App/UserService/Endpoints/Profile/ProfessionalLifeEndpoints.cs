using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Profile;
using UserService.Repositories.Profile;
using UserService.Services.Profile;
using UserService.Validators;

namespace UserService.Endpoints.Profile;

public class ProfessionalLifeEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("profile/professionallife", RegisterProfessionalLife)
            .RequireAuthorization("user_profile_stage");
        app.MapPut("profile/professionallife", UpdateProfessionalLife)
            .RequireAuthorization("completed_profile");
        app.MapGet("profile/professionallife", GetProfessionalLife)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IProfessionalLifeRepo, ProfessionalLifeRepo>();
        services.AddScoped<IProfessionalLifeService, ProfessionalLifeService>();
    }

    private async Task<IResult> RegisterProfessionalLife([FromServices] IProfessionalLifeService professionalLifeService, [FromBody] ProfessionalLifeDto userData)
    {
        var validator = new ProfessionalLifeDtoValidator();
        var results = validator.Validate(userData);
        if (!results.IsValid)
            return Results.BadRequest(results.Errors);

        var result = await professionalLifeService.RegisterProfessionalLife(userData);
        return result.Match<IResult>(
                    data => Results.Ok(data),
                    exception => Results.BadRequest(exception?.Message)
                );
    }

    private async Task<IResult> UpdateProfessionalLife([FromServices] IProfessionalLifeService professionalLifeService, [FromBody] ProfessionalLifeDto userData)
    {
        var validator = new ProfessionalLifeDtoValidator();
        var results = validator.Validate(userData);
        if (!results.IsValid)
            return Results.BadRequest(results.Errors);

        var result = await professionalLifeService.UpdateProfessionalLife(userData);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> GetProfessionalLife([FromServices] IProfessionalLifeRepo professionalLifeRepo)
    {
        var result = await professionalLifeRepo.GetProfessionalLife();
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }
}
