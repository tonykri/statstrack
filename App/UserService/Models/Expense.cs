using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class Expense
{
    [Key]
    public Guid UserId { get; set; }
    public User User { get; set; }

    [Key]
    public string UserExpense { get; set; }

    public Expense()
    {
    }
    public Expense(User user, string expense)
    {
        User = user;
        UserExpense = expense;
    }
}