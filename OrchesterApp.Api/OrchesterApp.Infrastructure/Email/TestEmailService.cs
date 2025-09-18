using System.Text;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;

namespace OrchesterApp.Infrastructure.Email;

public class TestEmailService : IEmailService
{
    public async Task SendEmailAsync(Message message)
    {
        foreach (var senderMail in message.To)
        {
            var mailPath = GetFakeMailAddressPath(senderMail);
            if (!Directory.Exists(mailPath))
            {
                Directory.CreateDirectory(mailPath);
            }

            await using var file = File.Create(Path.Combine(mailPath, $"{message.Subject}_{Guid.NewGuid()}.txt"));
            await file.WriteAsync(Encoding.UTF8.GetBytes(message.Content), CancellationToken.None);
        }
    }

    private string GetFakeMailAddressPath(string senderMail)
    {
        return Path.Combine(Environment.CurrentDirectory, "TestMails", senderMail);
    }
}