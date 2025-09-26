using Microsoft.Extensions.Logging;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.UserAggregate;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Common.Services;

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

            var message = new Message([user.Email], "Termindaten haben sich geändert",
                $"Die Zeit hat sich von {changeTerminDataNotification.OldStartZeit} auf {changeTerminDataNotification.NewStartZeit} geändert");

            result.Add(message);
        }

        return result;
    }
}