using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Notifications;
using OrchesterApp.Domain.UserAggregate;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Features.Notification.NotificationCategoryEmailSender;

public class CustomMessageEmailSender : INotificationCategoryEmailSender
{
    private readonly ILogger<CustomMessageEmailSender> _logger;

    public CustomMessageEmailSender(ILogger<CustomMessageEmailSender> logger)
    {
        _logger = logger;
    }

    public NotificationCategory NotificationCategory => NotificationCategory.CustomMessage;

    public List<Message> CreateMessage(OrchesterApp.Domain.NotificationAggregate.Notification notification,
        IList<UserNotification> userNotifications,
        Dictionary<UserNotification, User?> userNotificationsDict)
    {
        if (notification is not CustomMessageNotification customMessageNotification)
        {
            _logger.LogError("Invalid notification type.");
            return [];
        }

        var result = new List<Message>();

        foreach (var userNotification in userNotifications)
        {
            if (!userNotificationsDict.TryGetValue(userNotification, out var user) || user?.Email is null)
            {
                continue;
            }

            var textContent = BuildTextContent(customMessageNotification);
            var htmlContent = BuildHtmlContent(customMessageNotification);

            var message = new Message([user.Email], customMessageNotification.Title, textContent, htmlContent);

            result.Add(message);
        }

        return result;
    }

    private static string BuildTextContent(CustomMessageNotification notification)
    {
        var content = new StringBuilder();
        content.AppendLine("Hallo!");
        content.AppendLine();
        content.AppendLine(notification.Message);
        content.AppendLine();
        content.AppendLine("Mit freundlichen Grüßen,");
        content.AppendLine("Das Orchester App Team");
        content.AppendLine();
        content.AppendLine("---");
        content.AppendLine("Diese E-Mail wurde automatisch generiert. Bitte antworten Sie nicht auf diese E-Mail.");

        return content.ToString();
    }

    private static string BuildHtmlContent(CustomMessageNotification notification)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName =
                "TvJahnOrchesterApp.Application.Features.Termin.NotificationCategoryEmailSender.CustomMessageEmailTemplate.html";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
            }

            using var reader = new StreamReader(stream);
            var template = reader.ReadToEnd();

            return template
                .Replace("{{TITLE}}", notification.Title)
                .Replace("{{MESSAGE}}", notification.Message);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to load email template.", ex);
        }
    }
}


