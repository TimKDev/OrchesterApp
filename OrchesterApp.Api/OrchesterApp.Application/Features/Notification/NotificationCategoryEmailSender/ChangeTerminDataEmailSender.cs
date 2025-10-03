using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.NotificationAggregate.Notifications;
using OrchesterApp.Domain.UserAggregate;
using OrchesterApp.Domain.UserNotificationAggregate;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Features.Notification.NotificationCategoryEmailSender;

public class ChangeTerminDataEmailSender : INotificationCategoryEmailSender
{
    private readonly ILogger<ChangeTerminDataEmailSender> _logger;

    public ChangeTerminDataEmailSender(ILogger<ChangeTerminDataEmailSender> logger)
    {
        _logger = logger;
    }

    public NotificationCategory NotificationCategory => NotificationCategory.ChangeTerminData;

    public List<Message> CreateMessage(OrchesterApp.Domain.NotificationAggregate.Notification notification,
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
            var htmlContent = BuildHtmlContent(changesContent, changeTerminDataNotification.Author, 
                changeTerminDataNotification.TerminName, changeTerminDataNotification.TerminStartZeit);

            var emailSubject = $"Termindaten haben sich geändert: {changeTerminDataNotification.TerminName}";
            var message = new Message([user.Email], emailSubject, textContent, htmlContent);

            result.Add(message);
        }

        return result;
    }

    private static string BuildChangesContent(ChangeTerminDataNotification notification)
    {
        var changes = new StringBuilder();

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

        if (notification.HasTreffpunktChanged)
        {
            changes.AppendLine(
                $"<div class=\"change-item\">" +
                $"<strong>Treffpunkt:</strong> " +
                $"<span class=\"new-value\">Treffpunkt wurde verändert</span>" +
                $"</div>");
        }

        if (notification.HasDokumentChanged)
        {
            changes.AppendLine(
                $"<div class=\"change-item\">" +
                $"<strong>Dokument:</strong> " +
                $"<span class=\"new-value\">Dokument wurde verändert</span>" +
                $"</div>");
        }

        if (notification.HasUniformChanged)
        {
            changes.AppendLine(
                $"<div class=\"change-item\">" +
                $"<strong>Uniform:</strong> " +
                $"<span class=\"new-value\">Uniform wurde verändert</span>" +
                $"</div>");
        }

        if (notification.HasNotenChanged)
        {
            changes.AppendLine(
                $"<div class=\"change-item\">" +
                $"<strong>Noten:</strong> " +
                $"<span class=\"new-value\">Noten wurden verändert</span>" +
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
        content.AppendLine(
            $"Die Daten für den Termin '{notification.TerminName}' am {notification.TerminStartZeit:dd.MM.yyyy} wurden von {notification.Author} geändert. Hier sind die Details:");
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

        if (notification.HasTreffpunktChanged)
        {
            content.AppendLine("Treffpunkt: Treffpunkt wurde verändert");
        }

        if (notification.HasDokumentChanged)
        {
            content.AppendLine("Dokument: Dokument wurde verändert");
        }

        if (notification.HasUniformChanged)
        {
            content.AppendLine("Uniform: Uniform wurde verändert");
        }

        if (notification.HasNotenChanged)
        {
            content.AppendLine("Noten: Noten wurden verändert");
        }

        content.AppendLine();
        content.AppendLine("Mit freundlichen Grüßen,");
        content.AppendLine("Das Orchester App Team");
        content.AppendLine();
        content.AppendLine("---");
        content.AppendLine("Diese E-Mail wurde automatisch generiert. Bitte antworten Sie nicht auf diese E-Mail.");

        return content.ToString();
    }

    private static string BuildHtmlContent(string changesContent, string author, string terminName, DateTime terminStartZeit)
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

            return template
                .Replace("{{CHANGES_CONTENT}}", changesContent)
                .Replace("{{Author}}", author)
                .Replace("{{TERMIN_NAME}}", terminName)
                .Replace("{{TERMIN_START_ZEIT}}", terminStartZeit.ToString("dd.MM.yyyy"));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to load email template.", ex);
        }
    }
}