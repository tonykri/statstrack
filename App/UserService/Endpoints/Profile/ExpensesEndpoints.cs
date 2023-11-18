using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using UserService.Dto.Profile;
using UserService.Repositories.Profile;
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
    }

    private IResult RegisterExpenses([FromServices] IExpensesRepo expensesRepo, [FromBody] ExpensesDto userData)
    {
        var validator = new ExpensesDtoValidator();
        var result = validator.Validate(userData);
        if(!result.IsValid)
            return Results.BadRequest(result.Errors);
        try
        {
            string token = expensesRepo.RegisterExpenses(userData);
            return Results.Ok(token);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult UpdateExpenses([FromServices] IExpensesRepo expensesRepo, [FromBody] ExpensesDto userData)
    {
        var validator = new ExpensesDtoValidator();
        var result = validator.Validate(userData);
        if(!result.IsValid)
            return Results.BadRequest(result.Errors);
        try
        {
            expensesRepo.UpdateExpenses(userData);
            return Results.Ok();
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult GetExpenses([FromServices] IExpensesRepo expensesRepo)
    {
        try
        {
            var expenses = expensesRepo.GetExpenses();
            return Results.Ok(expenses);
        }catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
