using Microsoft.EntityFrameworkCore;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Profile;

public class ExpensesService : IExpensesService
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    public ExpensesService(ITokenDecoder tokenDecoder, DataContext dataContext, JwtToken jwtToken)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
    }

    private async Task<ApiResponse<string, Exception>> HandleExpenses(ExpensesDto userData, Guid userId, string action)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return new ApiResponse<string, Exception>(new Exception(ExceptionMessages.NOT_FOUND));

        if (action.Equals("update"))
        {
            var expenses = dataContext.Expenses.Where(e => e.UserId.ToString().Equals(user.Id.ToString()));
            dataContext.Expenses.RemoveRange(expenses);
        }

        foreach (string expense in userData.Expenses)
            await dataContext.Expenses.AddAsync(new Expense(user, expense));

        if (action.Equals("register"))
            user.ProfileStage = ProfileStages.Completed.ToString();
        await dataContext.SaveChangesAsync();

        string msg = action.Equals("register") ? jwtToken.CreateLoginToken(user) : "Update completed";
        return new ApiResponse<string, Exception>(msg);
    }

    public async Task<ApiResponse<string, Exception>> RegisterExpenses(ExpensesDto userData)
    {
        Guid userId = tokenDecoder.GetUserId();
        string action = "register";
        return await HandleExpenses(userData, userId, action);
    }

    public async Task<ApiResponse<string, Exception>> UpdateExpenses(ExpensesDto userData)
    {
        Guid userId = tokenDecoder.GetUserId();
        string action = "update";
        return await HandleExpenses(userData, userId, action);
    }

}