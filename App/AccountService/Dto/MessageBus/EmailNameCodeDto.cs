namespace AccountService.Dto.MessageBus;

public class EmailNameCodeDto
{
    public EmailNameCodeDto(string email, string name, string code, string eventType)
    {
        Email = email;
        Name = name;
        Code = code;
        Event = eventType;
    }

    public string Email { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Event { get; set; }
}
