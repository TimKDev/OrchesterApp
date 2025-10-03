using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.ValueObjects;
using OrchesterApp.Domain.UserAggregate.ValueObjects;
using OrchesterApp.Domain.UserNotificationAggregate;
using OrchesterApp.Domain.UserNotificationAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface IUserNotificationRepository
    {
        Task Save(IList<UserNotification> userNotification, CancellationToken cancellationToken);
        Task<UserNotification> GetById(UserNotificationId id, CancellationToken cancellationToken);
        Task<List<UserNotification>> GetByIds(List<UserNotificationId> ids, CancellationToken cancellationToken);
        Task<UserNotification[]> GetByUserId(UserId userId, int limitResult, CancellationToken cancellationToken);

        Task<UserNotification[]>
            GetByNotificationIds(IList<NotificationId> notificationId, CancellationToken cancellationToken);

        Task<bool> Delete(UserNotificationId id, CancellationToken cancellationToken);
    }
}