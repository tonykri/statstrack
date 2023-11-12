using UserService.Categories;
using UserService.Dto.Profile;
using UserService.Models;
using UserService.Utils;

namespace UserService.Repositories.Profile;

public class ExpensesRepo : IExpensesRepo
{
    private readonly ITokenDecoder tokenDecoder;
    private readonly DataContext dataContext;
    private readonly JwtToken jwtToken;
    public ExpensesRepo(ITokenDecoder tokenDecoder, DataContext dataContext, JwtToken jwtToken)
    {
        this.tokenDecoder = tokenDecoder;
        this.dataContext = dataContext;
        this.jwtToken = jwtToken;
    }

    private string HandleExpenses(ExpensesDto userData, Guid userId, string action)
    {
        var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
        if(user is null)
            throw new NotFoundException("User not found");

        if(action.Equals("update"))
        {
            var expenses = dataContext.Expenses.Where(e => e.UserId.ToString().Equals(user.Id.ToString()));
            dataContext.Expenses.RemoveRange(expenses);
        }

        foreach(string expense in userData.Expenses)
            dataContext.Expenses.Add(new Expense(user, expense));
        
        if(action.Equals("register"))
            user.ProfileStage = ProfileStages.Completed.ToString();
        dataContext.SaveChanges();

        return action.Equals("register") ? jwtToken.CreateLoginToken(user): "Update completed";
    }

    public string RegisterExpenses(ExpensesDto userData)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            string action = "register";
            return HandleExpenses(userData, userId, action);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void UpdateExpenses(ExpensesDto userData)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            string action = "update";
            HandleExpenses(userData, userId, action);
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public ICollection<string> GetExpenses()
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            return dataContext.Expenses
                .Where(e => e.UserId == userId)
                .Select(e => e.UserExpense)
                .ToList();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}