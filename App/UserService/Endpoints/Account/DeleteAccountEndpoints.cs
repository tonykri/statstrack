using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Services.Account;

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
        services.AddScoped<IDeleteAccountService, DeleteAccountService>();
    }

    private async Task<IResult> SendDeletionEmail([FromServices] IDeleteAccountService deleteAccountService)
    {
        var result = await deleteAccountService.SendDeletionEmail();
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => {
                if (exception?.Message == ExceptionMessages.NOT_FOUND)
                    return Results.NotFound(exception?.Message);
                else
                    return Results.BadRequest(exception?.Message);
            }
        );
    }

    private async Task<IResult> DeleteAccount([FromServices] IDeleteAccountService deleteAccountService, [FromRoute] string code)
    {
        var result = await deleteAccountService.DeleteAccount(code);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception => {
                if (exception?.Message == ExceptionMessages.NOT_FOUND)
                    return Results.NotFound(exception?.Message);
                else
                    return Results.BadRequest(exception?.Message);
            }
        );
    }
}
