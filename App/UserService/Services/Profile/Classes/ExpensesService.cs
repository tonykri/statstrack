using Microsoft.EntityFrameworkCore;
using UserService.AsymcDataProcessing.MessageBusClient;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.MessageBus.Send;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Services.Profile;

public class ExpensesService : IExpensesService
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    private readonly IMessageBusClient messageBusClient;
    public ExpensesService(ITokenDecoder tokenDecoder, DataContext dataContext, JwtToken jwtToken, IMessageBusClient messageBusClient)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
        this.messageBusClient = messageBusClient;
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
        {
            user.ProfileStage = ProfileStages.Completed.ToString();

            var message = new ProfileStageUpdatedDto(user.Id, ProfileStages.Completed);
            messageBusClient.Send(ref message);
        }
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