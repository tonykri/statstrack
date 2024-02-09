using UserService.Dto;
using UserService.Dto.Profile;

namespace UserService.Services.Profile;

public interface IExpensesService
{
    Task<ApiResponse<string, Exception>> RegisterExpenses(ExpensesDto userData);
    Task<ApiResponse<string, Exception>> UpdateExpenses(ExpensesDto userData);
}