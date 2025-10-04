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

public class TerminMissingResponseEmailSender : INotificationCategoryEmailSender
{
    private readonly ILogger<TerminMissingResponseEmailSender> _logger;
    private readonly string _frontendUrl;

    public TerminMissingResponseEmailSender(ILogger<TerminMissingResponseEmailSender> logger, IConfiguration configuration)
    {
        _logger = logger;
        _frontendUrl = configuration["FrontendUrl"] ?? "http://localhost:8100";
    }

    public NotificationCategory NotificationCategory => NotificationCategory.TerminMissingResponse;

    public List<Message> CreateMessage(OrchesterApp.Domain.NotificationAggregate.Notification notification,
        IList<UserNotification> userNotifications,
        Dictionary<UserNotification, User?> userNotificationsDict)
    {
        if (notification is not TerminMissingResponseNotification missingResponseNotification)
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

            var terminLink = $"{_frontendUrl}/tabs/termin/details/{missingResponseNotification.TerminId!.Value}/overview";
            var textContent = BuildTextContent(missingResponseNotification, terminLink);
            var htmlContent = BuildHtmlContent(missingResponseNotification, terminLink);

            var emailSubject = $"Fehlende Rückmeldung: {missingResponseNotification.TerminName}";
            var message = new Message([user.Email], emailSubject, textContent, htmlContent);

            result.Add(message);
        }

        return result;
    }

    private static string BuildTextContent(TerminMissingResponseNotification notification, string terminLink)
    {
        var content = new StringBuilder();
        content.AppendLine("Hallo!");
        content.AppendLine();
        content.AppendLine(
            $"Die Frist für die Rückmeldung zum Termin '{notification.TerminName}' ist abgelaufen, und wir haben noch keine Rückmeldung von dir erhalten.");
        content.AppendLine();
        content.AppendLine($"Termin: {notification.TerminDate:dd.MM.yyyy HH:mm}");
        content.AppendLine();
        content.AppendLine("Bitte melde dich sobald wie möglich zurück, ob du an diesem Termin teilnehmen kannst.");
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

    private static string BuildHtmlContent(TerminMissingResponseNotification notification, string terminLink)
    {
        var template = File.ReadAllText(Path.Combine(AppContext.BaseDirectory,
            "Features/Notification/NotificationCategoryEmailSender/TerminMissingResponseEmailTemplate.html"));

        return template
            .Replace("{{TERMIN_NAME}}", notification.TerminName)
            .Replace("{{TERMIN_DATE}}", notification.TerminDate.ToString("dd.MM.yyyy HH:mm"))
            .Replace("{{TERMIN_LINK}}", terminLink);
    }
}

