using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Profile;
using UserService.Repositories.Profile;
using UserService.Services.Profile;
using UserService.Validators;

namespace UserService.Endpoints.Profile;

public class ExpensesEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("profile/expenses", RegisterExpenses)
            .RequireAuthorization("hobbies_profile_stage");
        app.MapPut("profile/expenses", UpdateExpenses)
            .RequireAuthorization("completed_profile");
        app.MapGet("profile/expenses", GetExpenses)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IExpensesRepo, ExpensesRepo>();
        services.AddScoped<IExpensesService, ExpensesService>();
    }

    private async Task<IResult> RegisterExpenses([FromServices] IExpensesService expensesService, [FromBody] ExpensesDto userData)
    {
        var validator = new ExpensesDtoValidator();
        var validatorResult = validator.Validate(userData);
        if (!validatorResult.IsValid)
            return Results.BadRequest(validatorResult.Errors);

        var result = await expensesService.RegisterExpenses(userData);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> UpdateExpenses([FromServices] IExpensesService expensesService, [FromBody] ExpensesDto userData)
    {
        var validator = new ExpensesDtoValidator();
        var validatorResult = validator.Validate(userData);
        if (!validatorResult.IsValid)
            return Results.BadRequest(validatorResult.Errors);

        var result = await expensesService.UpdateExpenses(userData);
        return result.Match<IResult>(
            data => Results.Ok(data),
            exception => Results.BadRequest(exception?.Message)
        );
    }

    private async Task<IResult> GetExpenses([FromServices] IExpensesRepo expensesRepo)
    {
        var expenses = await expensesRepo.GetExpenses();
        return Results.Ok(expenses);
    }
}
