using Microsoft.Extensions.Logging;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Notifications;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Notification.Interfaces;

namespace TvJahnOrchesterApp.Application.Features.Notification.PortalNotificationBuilder;

public class ChangeTerminDataPortalCategoryBuilder : IPortalCategoryNotificationBuilder
{
    private readonly ITerminRepository _terminRepository;
    private readonly ILogger<ChangeTerminDataPortalCategoryBuilder> _logger;

    public ChangeTerminDataPortalCategoryBuilder(ITerminRepository terminRepository,
        ILogger<ChangeTerminDataPortalCategoryBuilder> logger)
    {
        _terminRepository = terminRepository;
        _logger = logger;
    }

    public NotificationCategory NotificationCategory => NotificationCategory.ChangeTerminData;

    public async Task<PortalNotificationContent> BuildAsync(
        OrchesterApp.Domain.NotificationAggregate.Notification notification, CancellationToken cancellationToken)
    {
        if (notification is not ChangeTerminDataNotification changeTerminDataNotification)
        {
            throw new ApplicationException("Invalid notification type");
        }

        if (changeTerminDataNotification.TerminId is null)
        {
            _logger.LogError("ChangeTerminNotification {notificationId} has no TerminId set.", notification.Id.Value);

            return changeTerminDataNotification.GetPortalNotificationContent(null, null);
        }

        var termin = await _terminRepository.GetById(changeTerminDataNotification.TerminId.Value, cancellationToken);

        return changeTerminDataNotification.GetPortalNotificationContent(termin?.Name, termin?.EinsatzPlan.StartZeit);
    }
}