using Microsoft.EntityFrameworkCore;
using UserService.Categories;
using UserService.Dto;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class ExpensesRepo : IExpensesRepo
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    public ExpensesRepo(ITokenDecoder tokenDecoder, DataContext dataContext)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
    }

    public async Task<ICollection<string>> GetExpenses()
    {
        Guid userId = tokenDecoder.GetUserId();
        var data = await dataContext.Expenses
            .Where(e => e.UserId == userId)
            .Select(e => e.UserExpense)
            .ToListAsync();
        return data;
    }
}