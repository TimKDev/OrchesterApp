using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using OrchesterApp.Domain.UserAggregate.ValueObjects;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Features.Termin.Endpoints;

namespace TvJahnOrchesterApp.Application.Common.Services;

public class NotifyService : INotifyService
{
    private readonly IOrchesterMitgliedRepository _orchesterMitgliedRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserNotificationRepository _userNotificationRepository;

    public NotifyService(IOrchesterMitgliedRepository orchesterMitgliedRepository,
        INotificationRepository notificationRepository, IUserNotificationRepository userNotificationRepository)
    {
        _orchesterMitgliedRepository = orchesterMitgliedRepository;
        _notificationRepository = notificationRepository;
        _userNotificationRepository = userNotificationRepository;
    }

    public async Task PublishNotificationAsync(Notification notification,
        List<OrchesterMitgliedsId> mitgliedsIds, NotificationSink[] notificationSinks,
        CancellationToken cancellationToken = default)
    {
        if (!notificationSinks.Any())
        {
            return;
        }

        var orchesterMitglieder =
            await _orchesterMitgliedRepository.QueryByIdsAsync(mitgliedsIds.ToArray(), cancellationToken);

        var userIdsForNotification = new List<UserId>();
        foreach (var orchesterMitglied in orchesterMitglieder)
        {
            if (Guid.TryParse(orchesterMitglied.ConnectedUserId, out var userId))
            {
                userIdsForNotification.Add(UserId.Create(userId));
            }
        }

        if (!userIdsForNotification.Any())
        {
            return;
        }

        await _notificationRepository.Save(notification, cancellationToken);

        var userNotificationsToSend = new List<UserNotification>();

        foreach (var userId in userIdsForNotification)
        {
            if (notificationSinks.Contains(NotificationSink.Portal))
            {
                userNotificationsToSend.Add(UserNotification.Create(userId, notification.Id,
                    NotificationSink.Portal));
            }

            if (notificationSinks.Contains(NotificationSink.Email))
            {
                userNotificationsToSend.Add(UserNotification.Create(userId, notification.Id,
                    NotificationSink.Email));
            }
        }

        await _userNotificationRepository.Save(userNotificationsToSend, cancellationToken);
    }
}