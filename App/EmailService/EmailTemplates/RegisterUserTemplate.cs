namespace EmailService.EmailTemplates;

public static class RegisterUserTemplate
{
 private static string mailHeader = @"
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
    
    public static string Body(string name, string code)
    {
        return mailHeader + @"
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
    }   
}