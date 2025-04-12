using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;
using TvJahnOrchesterApp.Application.Features.Authorization.Interfaces;
using OrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Services
{
    //Common Send Verification Email Logic => Kann wiederverwendet werden innerhalb des Features
    public class VerificationEmailService: IVerificationEmailService
    {
        private readonly IEmailService emailService;
        private readonly UserManager<User> userManager;

        public VerificationEmailService(IEmailService emailService, UserManager<User> userManager)
        {
            this.emailService = emailService;
            this.userManager = userManager;
        }

        public async Task SendTo(User user, string clientUri)
        {
            var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
                {
                    {"token", emailConfirmationToken },
                    {"email", user.Email }
                };
            var callback = QueryHelpers.AddQueryString(clientUri, param);
            var message = new Message(new string[] { user.Email }, "Email Confirmation token", callback);
            await emailService.SendEmailAsync(message);
        }
    }
}
