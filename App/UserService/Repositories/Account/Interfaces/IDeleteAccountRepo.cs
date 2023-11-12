namespace UserService.Repositories.Account;

public interface IDeleteAccountRepo
{
    void SendDeletionEmail();
    void DeleteAccount(string code);
}