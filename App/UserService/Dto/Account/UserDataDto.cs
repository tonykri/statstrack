using System.Text.Json.Serialization;
using UserService.Models;

namespace UserService.Dto.Account;

public class UserDataDto
{
    public UserDataDto(User user, string token)
    {
        this.user = new UserDto(user);
        this.token = token;
    }
    public UserDto user { get; set; }
    public string token { get; set; }
}