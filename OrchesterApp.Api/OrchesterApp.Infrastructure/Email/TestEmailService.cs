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
            await using var file = File.Create(Path.Combine(Environment.CurrentDirectory, "TestMails",
                ComputeFileName(message, senderMail)));
            await file.WriteAsync(Encoding.UTF8.GetBytes(message.Content), CancellationToken.None);
        }
    }

    private static string ComputeFileName(Message message, string senderMail)
    {
        var uniqueEnd = Guid.NewGuid().ToString().Skip(8).Take(4);
        return $"{senderMail}_{message.Subject}_{uniqueEnd}.txt";
    }
}