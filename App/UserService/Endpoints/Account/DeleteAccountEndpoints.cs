using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Repositories.Account;

namespace UserService.Endpoints.Account;

public class DeleteAccountEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("account/delete", SendDeletionEmail)
            .RequireAuthorization("completed_profile");
        app.MapDelete("account/delete/{code}", DeleteAccount)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IDeleteAccountRepo, DeleteAccountRepo>();
    }

    private IResult SendDeletionEmail([FromServices] IDeleteAccountRepo deleteAccountRepo)
    {
        try
        {
            deleteAccountRepo.SendDeletionEmail();
            return Results.NoContent();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult DeleteAccount([FromServices] IDeleteAccountRepo deleteAccountRepo, [FromRoute] string code)
    {
        try
        {
            deleteAccountRepo.DeleteAccount(code);
            return Results.NoContent();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
    }
}
