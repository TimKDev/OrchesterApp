using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.UserAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface IUserNotificationRepository
    {
        Task<UserNotification> Save(UserNotification userNotification, CancellationToken cancellationToken);
        Task<UserNotification> GetById(UserNotificationId id, CancellationToken cancellationToken);
        Task<UserNotification[]> GetByUserId(UserId userId, CancellationToken cancellationToken);
        Task<UserNotification[]> GetByNotificationId(NotificationId notificationId, CancellationToken cancellationToken);
        Task<bool> Delete(UserNotificationId id, CancellationToken cancellationToken);
    }
} 