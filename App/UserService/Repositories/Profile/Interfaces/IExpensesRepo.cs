using UserService.Dto.Profile;

namespace UserService.Repositories.Profile;

public interface IExpensesRepo
{
    Task<ICollection<string>> GetExpenses();
}