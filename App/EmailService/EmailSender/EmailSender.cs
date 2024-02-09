using System.Net;
using System.Net.Mail;

namespace EmailService;

public class EmailSender : IEmailSender
{
    private readonly string SMTP_SERVER = "smtp.gmail.com";
    private readonly int SMTP_PORT = 587;
    private readonly string FROM_EMAIL, FROM_PASSWORD;
    
    public EmailSender(IConfiguration configuration)
    {
        FROM_EMAIL = configuration.GetSection("EmailSettings:Email").Value;
        FROM_PASSWORD = configuration.GetSection("EmailSettings:Password").Value;
    }

    public void Send(string toEmail, string mailBody)
    {
        // Create and configure the SMTP client
        SmtpClient client = new SmtpClient(SMTP_SERVER)
        {
            Port = SMTP_PORT,
            Credentials = new NetworkCredential(FROM_EMAIL, FROM_PASSWORD),
            EnableSsl = true
        };

        // Create the email message
        MailMessage mailMessage = new MailMessage
        {
            From = new MailAddress(FROM_EMAIL)
        };
        mailMessage.To.Add(toEmail);
        mailMessage.Subject = "Confirmation Code";
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = mailBody;

        try
        {
            // Send the email
            client.Send(mailMessage);
            Console.WriteLine("Email sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }
}