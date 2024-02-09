using System.Text.Json;
using EmailService.Dto;
using EmailService.EmailTemplates;

namespace EmailService.AsymcDataProcessing.EventHandling;

public class EventHandler : IEventHandler
{
    private readonly IEmailSender emailSender;
    public EventHandler(IEmailSender emailSender)
    {
        this.emailSender = emailSender;
    }

    public void DeleteAccount(string message)
    {
        var eventDto = JsonSerializer.Deserialize<EmailNameCodeDto>(message);
        if (eventDto is null) return;
        emailSender.Send(eventDto.Email, DeleteAccountTemplate.Body(eventDto.Name, eventDto.Code));
        Console.WriteLine("--> Delete Account Email Sent");
    }

    public void LoginUser(string message)
    {
        var eventDto = JsonSerializer.Deserialize<EmailNameCodeDto>(message);
        if (eventDto is null) return;
        emailSender.Send(eventDto.Email, LoginUserTemplate.Body(eventDto.Name, eventDto.Code));
        Console.WriteLine("--> Login User Email Sent");
    }

    public void RegisterUser(string message)
    {
        var eventDto = JsonSerializer.Deserialize<EmailNameCodeDto>(message);
        if (eventDto is null) return;
        emailSender.Send(eventDto.Email, RegisterUserTemplate.Body(eventDto.Name, eventDto.Code));
        Console.WriteLine("--> Register User Email Sent");
    }
}
