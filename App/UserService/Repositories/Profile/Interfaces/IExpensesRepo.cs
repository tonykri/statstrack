using UserService.Dto.Profile;

namespace UserService.Repositories.Profile;

public interface IExpensesRepo
{
    string RegisterExpenses(ExpensesDto userData);
    void UpdateExpenses(ExpensesDto userData);
    ICollection<string> GetExpenses();
}