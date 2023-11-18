using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Profile;
using UserService.Repositories.Profile;
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
    }

    private IResult RegisterProfessionalLife([FromServices] IProfessionalLifeRepo professionalLifeRepo, [FromBody] ProfessionalLifeDto userData)
    {
        var validator = new ProfessionalLifeDtoValidator();
        var results = validator.Validate(userData);
        if(!results.IsValid)
            return Results.BadRequest(results.Errors);
        try
        {
            string token = professionalLifeRepo.RegisterProfessionalLife(userData);
            return Results.Ok(token);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult UpdateProfessionalLife([FromServices] IProfessionalLifeRepo professionalLifeRepo, [FromBody] ProfessionalLifeDto userData)
    {
        var validator = new ProfessionalLifeDtoValidator();
        var results = validator.Validate(userData);
        if(!results.IsValid)
            return Results.BadRequest(results.Errors);
        try
        {
            professionalLifeRepo.UpdateProfessionalLife(userData);
            return Results.Ok();
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult GetProfessionalLife([FromServices] IProfessionalLifeRepo professionalLifeRepo)
    {
        try
        {
            var professionalLife = professionalLifeRepo.GetProfessionalLife();
            return Results.Ok(professionalLife);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
