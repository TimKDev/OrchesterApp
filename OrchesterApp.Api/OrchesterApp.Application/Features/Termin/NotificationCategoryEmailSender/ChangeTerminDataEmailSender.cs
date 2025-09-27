using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.UserAggregate;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Features.Termin.NotificationCategoryEmailSender;

public class ChangeTerminDataEmailSender : INotificationCategoryEmailSender
{
    private readonly ILogger<ChangeTerminDataEmailSender> _logger;

    public ChangeTerminDataEmailSender(ILogger<ChangeTerminDataEmailSender> logger)
    {
        _logger = logger;
    }

    public NotificationCategory NotificationCategory => NotificationCategory.ChangeTerminData;

    public List<Message> CreateMessage(Notification notification,
        IList<UserNotification> userNotifications,
        Dictionary<UserNotification, User?> userNotificationsDict)
    {
        if (notification is not ChangeTerminDataNotification changeTerminDataNotification)
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

            var changesContent = BuildChangesContent(changeTerminDataNotification);
            var textContent = BuildTextContent(changeTerminDataNotification);
            var htmlContent = BuildHtmlContent(changesContent);

            var message = new Message([user.Email], "Termindaten haben sich geändert", textContent, htmlContent);

            result.Add(message);
        }

        return result;
    }

    private static string BuildChangesContent(ChangeTerminDataNotification notification)
    {
        var changes = new StringBuilder();

        // Start time change
        if (notification.OldStartZeit.HasValue && notification.NewStartZeit.HasValue)
        {
            changes.AppendLine(
                $"<div class=\"change-item\">" +
                $"<strong>Startzeit:</strong> " +
                $"<span class=\"old-value\">{notification.OldStartZeit.Value:dd.MM.yyyy HH:mm}</span>" +
                $"<span class=\"arrow\">→</span>" +
                $"<span class=\"new-value\">{notification.NewStartZeit.Value:dd.MM.yyyy HH:mm}</span>" +
                $"</div>");
        }

        // End time change
        if (notification.OldEndZeit.HasValue && notification.NewEndZeit.HasValue)
        {
            changes.AppendLine(
                $"<div class=\"change-item\">" +
                $"<strong>Endzeit:</strong> " +
                $"<span class=\"old-value\">{notification.OldEndZeit.Value:dd.MM.yyyy HH:mm}</span>" +
                $"<span class=\"arrow\">→</span>" +
                $"<span class=\"new-value\">{notification.NewEndZeit.Value:dd.MM.yyyy HH:mm}</span>" +
                $"</div>");
        }

        // Status change (if we have status mappings in the future)
        if (notification.OldTerminStatus.HasValue && notification.NewTerminStatus.HasValue)
        {
            changes.AppendLine(
                $"<div class=\"change-item\">" +
                $"<strong>Status:</strong> " +
                $"<span class=\"old-value\">Status {notification.OldTerminStatus.Value}</span>" +
                $"<span class=\"arrow\">→</span>" +
                $"<span class=\"new-value\">Status {notification.NewTerminStatus.Value}</span>" +
                $"</div>");
        }

        return changes.Length > 0
            ? changes.ToString()
            : "<div class=\"no-changes\">Keine spezifischen Änderungen verfügbar.</div>";
    }

    private static string BuildTextContent(ChangeTerminDataNotification notification)
    {
        var content = new StringBuilder();
        content.AppendLine("Hallo!");
        content.AppendLine();
        content.AppendLine("Die Daten für einen Termin wurden geändert. Hier sind die Details:");
        content.AppendLine();

        if (notification.OldStartZeit.HasValue && notification.NewStartZeit.HasValue)
        {
            content.AppendLine(
                $"Startzeit: {notification.OldStartZeit.Value:dd.MM.yyyy HH:mm} → {notification.NewStartZeit.Value:dd.MM.yyyy HH:mm}");
        }

        if (notification.OldEndZeit.HasValue && notification.NewEndZeit.HasValue)
        {
            content.AppendLine(
                $"Endzeit: {notification.OldEndZeit.Value:dd.MM.yyyy HH:mm} → {notification.NewEndZeit.Value:dd.MM.yyyy HH:mm}");
        }

        if (notification.OldTerminStatus.HasValue && notification.NewTerminStatus.HasValue)
        {
            content.AppendLine($"Status: {notification.OldTerminStatus.Value} → {notification.NewTerminStatus.Value}");
        }

        content.AppendLine();
        content.AppendLine("Mit freundlichen Grüßen,");
        content.AppendLine("Das Orchester App Team");
        content.AppendLine();
        content.AppendLine("---");
        content.AppendLine("Diese E-Mail wurde automatisch generiert. Bitte antworten Sie nicht auf diese E-Mail.");

        return content.ToString();
    }

    private static string BuildHtmlContent(string changesContent)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName =
                "TvJahnOrchesterApp.Application.Features.Termin.NotificationCategoryEmailSender.ChangeTerminDataEmailTemplate.html";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
            }

            using var reader = new StreamReader(stream);
            var template = reader.ReadToEnd();

            return template.Replace("{{CHANGES_CONTENT}}", changesContent);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to load email template.", ex);
        }
    }
}