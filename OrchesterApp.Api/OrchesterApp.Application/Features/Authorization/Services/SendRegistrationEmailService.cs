using Microsoft.AspNetCore.WebUtilities;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;
using TvJahnOrchesterApp.Application.Features.Authorization.Interfaces;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Services
{
    internal class SendRegistrationEmailService : ISendRegistrationEmailService
    {
        private readonly IEmailService emailService;

        public SendRegistrationEmailService(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task SendTo(string email, string registrationKey, string clientUri)
        {
            var param = new Dictionary<string, string?>
            {
                { "registrationKey", registrationKey },
                { "email", email }
            };
            var callback = QueryHelpers.AddQueryString(clientUri, param);
            var subject = "Willkommen bei der Orchester App - Registrierung abschließen";
            var plainTextContent = CreatePlainTextContent(callback);
            var htmlContent = CreateHtmlContent(callback);

            var message = new Message(
                [email],
                subject,
                plainTextContent,
                htmlContent,
                "Orchester App Team");

            await emailService.SendEmailAsync(message);
        }

        private string CreatePlainTextContent(string registrationLink)
        {
            return $@"Willkommen bei der Orchester App!

Sie wurden zur Orchester App eingeladen. Um Ihr Konto zu erstellen, klicken Sie bitte auf den folgenden Link:

{registrationLink}

Dieser Link ist aus Sicherheitsgründen nur begrenzte Zeit gültig.

Falls Sie diese E-Mail fälschlicherweise erhalten haben, können Sie sie einfach ignorieren.

Mit freundlichen Grüßen,
Das Orchester App Team

---
Diese E-Mail wurde automatisch generiert. Bitte antworten Sie nicht auf diese E-Mail.";
        }

        private string CreateHtmlContent(string registrationLink)
        {
            return $@"<!DOCTYPE html>
<html lang=""de"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Willkommen bei der Orchester App</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }}
        .header {{
            background-color: #1976d2;
            color: white;
            padding: 20px;
            text-align: center;
            border-radius: 5px 5px 0 0;
        }}
        .content {{
            background-color: #f9f9f9;
            padding: 20px;
            border: 1px solid #ddd;
        }}
        .button {{
            display: inline-block;
            padding: 12px 30px;
            background-color: #1976d2;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin: 20px 0;
            font-weight: bold;
        }}
        .footer {{
            background-color: #f1f1f1;
            padding: 15px;
            text-align: center;
            font-size: 12px;
            color: #666;
            border-radius: 0 0 5px 5px;
        }}
    </style>
</head>
<body>
    <div class=""header"">
        <h1>🎵 Willkommen bei der Orchester App!</h1>
    </div>
    
    <div class=""content"">
        <p>Hallo!</p>
        
        <p>Sie wurden zur Orchester App eingeladen. Um Ihr Konto zu erstellen und Zugang zu allen Funktionen zu erhalten, klicken Sie bitte auf den folgenden Button:</p>
        
        <div style=""text-align: center;"">
            <a href=""{registrationLink}"" class=""button"">Registrierung abschließen</a>
        </div>
        
        <p>Oder kopieren Sie diesen Link in Ihren Browser:</p>
        <p style=""word-break: break-all; background-color: #fff; padding: 10px; border: 1px solid #ddd; border-radius: 3px;"">
            {registrationLink}
        </p>
        
        <p><strong>Wichtiger Hinweis:</strong> Dieser Link ist aus Sicherheitsgründen nur begrenzte Zeit gültig.</p>
        
        <p>Falls Sie diese E-Mail fälschlicherweise erhalten haben, können Sie sie einfach ignorieren.</p>
        
        <p>Mit freundlichen Grüßen,<br>
        Das Orchester App Team</p>
    </div>
    
    <div class=""footer"">
        Diese E-Mail wurde automatisch generiert. Bitte antworten Sie nicht auf diese E-Mail.
    </div>
</body>
</html>";
        }
    }
}