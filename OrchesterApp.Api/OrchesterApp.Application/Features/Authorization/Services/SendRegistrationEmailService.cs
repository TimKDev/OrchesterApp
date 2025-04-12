using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;
using TvJahnOrchesterApp.Application.Features.Authorization.Interfaces;
using OrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Services
{
    internal class SendRegistrationEmailService: ISendRegistrationEmailService
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
                    {"registrationKey", registrationKey },
                    {"email", email }
                };
            var callback = QueryHelpers.AddQueryString(clientUri, param);
            var message = new Message(new string[] { email }, "Registrierunglink Orchester App", callback);
            await emailService.SendEmailAsync(message);
        }
    }
}
