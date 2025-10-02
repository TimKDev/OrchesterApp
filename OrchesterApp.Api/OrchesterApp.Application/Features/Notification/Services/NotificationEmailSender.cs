using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.UserAggregate;
using OrchesterApp.Domain.UserNotificationAggregate;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;

namespace TvJahnOrchesterApp.Application.Common.Services;

public class NotificationEmailSender : INotificationEmailSender
{
    private readonly Dictionary<NotificationCategory, INotificationCategoryEmailSender>
        _notificationCategoryEmailSender;

    private readonly ILogger<NotificationEmailSender> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public NotificationEmailSender(IEnumerable<INotificationCategoryEmailSender> notificationCategoryEmailSender,
        ILogger<NotificationEmailSender> logger, UserManager<User> userManager, IEmailService emailService)
    {
        _logger = logger;
        _userManager = userManager;
        _emailService = emailService;
        _notificationCategoryEmailSender =
            notificationCategoryEmailSender.ToDictionary(s => s.NotificationCategory, s => s);
    }

    public async Task SendEmailsForNotificationAsync(Notification notification,
        IList<UserNotification> userNotifications,
        CancellationToken cancellationToken)
    {
        if (!_notificationCategoryEmailSender.TryGetValue(notification.Category,
                out var notificationCategoryEmailSender))
        {
            _logger.LogInformation(
                "For notification category {notificationCategory} has not been registered for email notification.}",
                notification.Category);

            return;
        }

        var users = await _userManager.Users
            .Where(u => userNotifications.Select(user => user.UserId.ToString()).Contains(u.Id))
            .ToListAsync(cancellationToken);

        var userNotificationsDict =
            userNotifications.ToDictionary(u => u, u => users.FirstOrDefault(user => user.Id == u.UserId.ToString()));

        var messagesToSend = notificationCategoryEmailSender.CreateMessage(notification, userNotifications,
            userNotificationsDict);

        foreach (var message in messagesToSend)
        {
            await _emailService.SendEmailAsync(message);
        }
    }
}