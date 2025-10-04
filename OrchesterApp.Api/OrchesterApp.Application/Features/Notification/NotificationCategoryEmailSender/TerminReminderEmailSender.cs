using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.NotificationAggregate.Notifications;
using OrchesterApp.Domain.UserAggregate;
using OrchesterApp.Domain.UserNotificationAggregate;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Features.Notification.NotificationCategoryEmailSender;

public class TerminReminderEmailSender : INotificationCategoryEmailSender
{
    private readonly ILogger<TerminReminderEmailSender> _logger;
    private readonly string _frontendUrl;

    public TerminReminderEmailSender(ILogger<TerminReminderEmailSender> logger, IConfiguration configuration)
    {
        _logger = logger;
        _frontendUrl = configuration["FrontendUrl"] ?? "http://localhost:8100";
    }

    public NotificationCategory NotificationCategory => NotificationCategory.TerminReminderBeforeDeadline;

    public List<Message> CreateMessage(OrchesterApp.Domain.NotificationAggregate.Notification notification,
        IList<UserNotification> userNotifications,
        Dictionary<UserNotification, User?> userNotificationsDict)
    {
        if (notification is not TerminReminderNotification reminderNotification)
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

            var terminLink = $"{_frontendUrl}/tabs/termin/details/{reminderNotification.TerminId!.Value}/overview";
            var textContent = BuildTextContent(reminderNotification, terminLink);
            var htmlContent = BuildHtmlContent(reminderNotification, terminLink);

            var emailSubject = $"Erinnerung: Rückmeldung für {reminderNotification.TerminName} ausstehend";
            var message = new Message([user.Email], emailSubject, textContent, htmlContent);

            result.Add(message);
        }

        return result;
    }

    private static string BuildTextContent(TerminReminderNotification notification, string terminLink)
    {
        var content = new StringBuilder();
        content.AppendLine("Hallo!");
        content.AppendLine();
        content.AppendLine(
            $"Dies ist eine freundliche Erinnerung, dass du noch nicht auf den Termin '{notification.TerminName}' geantwortet hast.");
        content.AppendLine();
        content.AppendLine($"Termin: {notification.TerminDate:dd.MM.yyyy HH:mm}");
        content.AppendLine($"Rückmeldungsfrist: {notification.TerminDeadline:dd.MM.yyyy HH:mm}");
        content.AppendLine();
        content.AppendLine("Bitte melde dich so bald wie möglich zurück, ob du an diesem Termin teilnehmen kannst.");
        content.AppendLine();
        content.AppendLine($"Termindetails ansehen: {terminLink}");
        content.AppendLine();
        content.AppendLine("Mit freundlichen Grüßen,");
        content.AppendLine("Das Orchester App Team");
        content.AppendLine();
        content.AppendLine("---");
        content.AppendLine("Diese E-Mail wurde automatisch generiert. Bitte antworten Sie nicht auf diese E-Mail.");

        return content.ToString();
    }

    private static string BuildHtmlContent(TerminReminderNotification notification, string terminLink)
    {
        var template = File.ReadAllText(Path.Combine(AppContext.BaseDirectory,
            "Features/Notification/NotificationCategoryEmailSender/TerminReminderEmailTemplate.html"));

        return template
            .Replace("{{TERMIN_NAME}}", notification.TerminName)
            .Replace("{{TERMIN_DATE}}", notification.TerminDate.ToString("dd.MM.yyyy HH:mm"))
            .Replace("{{DEADLINE}}", notification.TerminDeadline.ToString("dd.MM.yyyy HH:mm"))
            .Replace("{{TERMIN_LINK}}", terminLink);
    }
}

