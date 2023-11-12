using System.Net;
using System.Net.Mail;
using UserService.Models;

namespace UserService.Utils;

public class EmailService : IEmailService
{
    private readonly IConfiguration configuration;
    private readonly DataContext dataContext;
    public EmailService(IConfiguration configuration, DataContext dataContext)
    {
        this.configuration = configuration;
        this.dataContext = dataContext;
    }
    public void EmailSender(string toEmail, string name, string code, string action)
    {
        string smtpServer = "smtp.gmail.com";
        int smtpPort = 587;
        string? fromEmail = configuration.GetSection("EmailSettings:Email").Value;
        string? fromPassword = configuration.GetSection("EmailSettings:Password").Value;
        if(fromEmail is null || fromPassword is null)
            throw new NotFoundException("Server error");

        // Create and configure the SMTP client
        SmtpClient client = new SmtpClient(smtpServer);
        client.Port = smtpPort;
        client.Credentials = new NetworkCredential(fromEmail, fromPassword);
        client.EnableSsl = true;

        // Create the email message
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(fromEmail);
        mailMessage.To.Add(toEmail);
        mailMessage.Subject = "Confirmation Code";
        mailMessage.IsBodyHtml = true;

        string mailBody = @"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                /* Add custom styles here */
                body {
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 0;
                }
                .container {
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                }
                .header {
                    background-color: #0077B6;
                    color: #fff;
                    text-align: center;
                    padding: 20px;
                }
                .content {
                    padding: 20px;
                }
                .code {
                    font-weight: bold;
                    text-decoration: underline;
                }
                
            </style>
        </head>";


        string mailBodyRegister = mailBody + @"
            <body>
                <div class=""container"">
                    <div class=""header"">
                        <h1>StatsTrack</h1>
                    </div>
                    <div class=""content"">
                        <h2>Hello Mr./Mrs. " + name + @",</h2>
                        <p> Welcome to StatsTrack!</p>
                        <p> Use the 6 digit code below to confirm your email:</p>
                        <p class=""code""> " + code + @"</p>
                        <p>The current code is valid for 2 minutes.</p>
                        <p>Best regards,<br>StatsTrack</p>
                    </div>
                </div>
            </body>
            </html>
            ";
        string mailBodyLogin = mailBody + @"
            <body>
                <div class=""container"">
                    <div class=""header"">
                        <h1>StatsTrack</h1>
                    </div>
                    <div class=""content"">
                        <h2>Hello Mr./Mrs. " + name + @",</h2>
                        <p> Welcome to StatsTrack!</p>
                        <p> Use the 6 digit code below to login:</p>
                        <p class=""code""> " + code + @"</p>
                        <p>The current code is valid for 2 minutes.</p>
                        <p>Best regards,<br>StatsTrack</p>
                    </div>
                </div>
            </body>
            </html>
            ";
        string mailBodyDelete = mailBody + @"
            <body>
                <div class=""container"">
                    <div class=""header"">
                        <h1>StatsTrack</h1>
                    </div>
                    <div class=""content"">
                        <h2>Hello Mr./Mrs. " + name + @",</h2>
                        <p> We received your request to delete your account</p>
                        <p> Use the 6 digit code below for confirmation:</p>
                        <p class=""code""> " + code + @"</p>
                        <p>The current code is valid for 2 minutes.</p>
                        <p>We hope to see you soon.</p>
                        <p>Best regards,<br>StatsTrack</p>
                    </div>
                </div>
            </body>
            </html>
            ";

        if(action.Equals("register"))
            mailMessage.Body = mailBodyRegister;
        else if(action.Equals("login"))
            mailMessage.Body = mailBodyLogin;
        else
            mailMessage.Body = mailBodyDelete;

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

    public void VerifyEmail(Guid userId, string code)
    {

        var user = dataContext.Users.FirstOrDefault(u => u.Id.ToString().Equals(userId.ToString()));
        if(user is null)
            throw new NotFoundException("User not found");
        var emailCode = dataContext.EmailCodes.FirstOrDefault(e => e.UserId.ToString().Equals(user.Id.ToString()));
        if(emailCode is null)
            throw new NotFoundException("Email's code not found");

        if (emailCode.CodeCreated.AddMinutes(2) < DateTime.Now)
            throw new NotValidException("Code is not valid after 2 minutes");

        if (!emailCode.Code.Equals(code))
            throw new NotValidException("Invalid confirmation code");
        
    }

    public string CodeGenerator(Guid userId)
    {
        string characterSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        Random random = new Random();

        char[] code = new char[6];
        for (int i = 0; i < 6; i++)
        {
            int index = random.Next(characterSet.Length);
            code[i] = characterSet[index];
        }
        string randomCode = new string(code);

        var user = dataContext.Users.FirstOrDefault(u => u.Id.ToString().Equals(userId.ToString()));
        if(user is null)
            throw new NotFoundException("User not found");

        var emailCode = dataContext.EmailCodes.FirstOrDefault(e => e.UserId.ToString().Equals(userId.ToString()));
        if (emailCode is not null)
        {
            emailCode.Code = randomCode;
            emailCode.CodeCreated = DateTime.Now;
        }
        else
        {
            dataContext.EmailCodes.Add(new EmailCode
            {
                UserId = user.Id,
                User = user,
                Code = randomCode,
                CodeCreated = DateTime.Now
            });
        }

        dataContext.SaveChanges();

        return randomCode;
    }
}