using Microsoft.AspNetCore.WebUtilities;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;
using TvJahnOrchesterApp.Application.Features.Authorization.Interfaces;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Services
{
    internal class SendPasswordResetEmailService : ISendPasswordResetEmailService
    {
        private readonly IEmailService emailService;

        public SendPasswordResetEmailService(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task SendTo(string email, string resetToken, string clientUri)
        {
            var param = new Dictionary<string, string?>
            {
                { "token", resetToken },
                { "email", email }
            };
            var callback = QueryHelpers.AddQueryString(clientUri, param);
            var subject = "Orchester App - Passwort zurücksetzen";
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

        private static string CreatePlainTextContent(string resetLink)
        {
            return $@"Passwort zurücksetzen - Orchester App

Hallo!

Sie haben eine Anfrage zum Zurücksetzen Ihres Passworts für die Orchester App gestellt.

Um ein neues Passwort zu erstellen, klicken Sie bitte auf den folgenden Link:

{resetLink}

Dieser Link ist aus Sicherheitsgründen nur begrenzte Zeit gültig.

Falls Sie diese Anfrage nicht gestellt haben, können Sie diese E-Mail einfach ignorieren. Ihr Passwort wird nicht geändert.

Mit freundlichen Grüßen,
Das Orchester App Team

---
Diese E-Mail wurde automatisch generiert. Bitte antworten Sie nicht auf diese E-Mail.";
        }

        private static string CreateHtmlContent(string resetLink)
        {
            return $@"<!DOCTYPE html>
<html lang=""de"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Passwort zurücksetzen - Orchester App</title>
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
            background-color: #f44336;
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
            background-color: #f44336;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin: 20px 0;
            font-weight: bold;
        }}
        .warning-box {{
            background-color: #fff3cd;
            border: 1px solid #ffeaa7;
            border-radius: 5px;
            padding: 15px;
            margin: 15px 0;
            border-left: 4px solid #f39c12;
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
        <h1>🔒 Passwort zurücksetzen</h1>
    </div>
    
    <div class=""content"">
        <p>Hallo!</p>
        
        <p>Sie haben eine Anfrage zum Zurücksetzen Ihres Passworts für die Orchester App gestellt.</p>
        
        <p>Um ein neues Passwort zu erstellen, klicken Sie bitte auf den folgenden Button:</p>
        
        <div style=""text-align: center;"">
            <a href=""{resetLink}"" class=""button"">Neues Passwort erstellen</a>
        </div>
        
        <p>Oder kopieren Sie diesen Link in Ihren Browser:</p>
        <p style=""word-break: break-all; background-color: #fff; padding: 10px; border: 1px solid #ddd; border-radius: 3px;"">
            {resetLink}
        </p>
        
        <div class=""warning-box"">
            <p><strong>⚠️ Wichtige Sicherheitshinweise:</strong></p>
            <ul>
                <li>Dieser Link ist nur begrenzte Zeit gültig</li>
                <li>Falls Sie diese Anfrage nicht gestellt haben, ignorieren Sie diese E-Mail</li>
                <li>Ihr aktuelles Passwort bleibt unverändert, bis Sie ein neues erstellen</li>
            </ul>
        </div>
        
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