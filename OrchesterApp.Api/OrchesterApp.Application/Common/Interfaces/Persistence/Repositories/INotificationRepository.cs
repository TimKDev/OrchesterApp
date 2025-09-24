using OrchesterApp.Domain.NotificationAggregate;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> Save(Notification notification, CancellationToken cancellationToken);
        Task<Notification> GetById(NotificationId id, CancellationToken cancellationToken);
        Task<bool> Delete(NotificationId id, CancellationToken cancellationToken);
    }
} 