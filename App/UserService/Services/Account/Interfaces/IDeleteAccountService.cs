using UserService.Dto;

namespace UserService.Services.Account;

public interface IDeleteAccountService
{
    Task<ApiResponse<int, Exception>> SendDeletionEmail();
    Task<ApiResponse<int, Exception>> DeleteAccount(string code);
}