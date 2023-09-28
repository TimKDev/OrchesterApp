using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(Message message)
        {
            //Write Mail
        }
    }
}
