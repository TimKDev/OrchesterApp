using MailKit.Net.Smtp;
using MimeKit;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;

namespace OrchesterApp.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            var senderName = message.FromName ?? "Orchester App";
            emailMessage.From.Add(new MailboxAddress(senderName, _emailConfig.From));

            emailMessage.To.AddRange(message.To.Select(e => new MailboxAddress("", e)));

            emailMessage.Subject = message.Subject;

            if (!string.IsNullOrEmpty(message.HtmlContent))
            {
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = message.Content;
                bodyBuilder.HtmlBody = message.HtmlContent;
                emailMessage.Body = bodyBuilder.ToMessageBody();
            }
            else
            {
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            }

            // Add headers to improve deliverability and authentication
            emailMessage.Headers.Add("X-Mailer", "Orchester App v1.0");
            emailMessage.Headers.Add("X-Priority", "3");

            // Add Message-ID for better tracking and authentication
            emailMessage.MessageId = $"<{Guid.NewGuid()}@{GetDomainFromEmail(_emailConfig.From)}>";

            // Add Return-Path for better bounce handling
            emailMessage.Headers.Add("Return-Path", _emailConfig.From);

            // Add List-Unsubscribe header (helps with spam filtering)
            var domain = GetDomainFromEmail(_emailConfig.From);
            emailMessage.Headers.Add("List-Unsubscribe", $"<mailto:unsubscribe@{domain}>");
            emailMessage.Headers.Add("List-Unsubscribe-Post", "List-Unsubscribe=One-Click");

            // Add Auto-Submitted header to indicate this is an automated email
            emailMessage.Headers.Add("Auto-Submitted", "auto-generated");

            // Add Precedence header for automated emails
            emailMessage.Headers.Add("Precedence", "bulk");

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        private static string GetDomainFromEmail(string email)
        {
            var atIndex = email.IndexOf('@');
            return atIndex > 0 ? email.Substring(atIndex + 1) : "localhost";
        }
    }
}