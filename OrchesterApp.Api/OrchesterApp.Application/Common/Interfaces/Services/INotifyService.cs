using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using OrchesterApp.Domain.UserNotificationAggregate.Enums;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Services;

public interface INotifyService
{
    Task PublishNotificationAsync(Notification notification,
        List<OrchesterMitgliedsId> mitgliedsIds,
        List<NotificationSink> selectedSinks, CancellationToken cancellationToken = default);
}