using AccountService.Models;

namespace AccountService.Utils;

public interface IJwtService 
{
    string CreateLoginToken(Account user);
    public Guid GetUserId();
}