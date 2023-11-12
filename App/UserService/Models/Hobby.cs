using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class Hobby 
{
    [Key]
    public Guid UserId { get; set; }
    public User User { get; set; }

    [Key]
    public string UserHobby { get; set; }

    public Hobby()
    {
    }
    public Hobby(User user, string hobby)
    {
        User = user;
        UserHobby = hobby;
    }
}