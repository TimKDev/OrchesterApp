using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> Save(Notification notification, CancellationToken cancellationToken);
        Task<Notification> GetById(NotificationId id, CancellationToken cancellationToken);
        Task<List<Notification>> GetByIds(IList<NotificationId> ids, CancellationToken cancellationToken);
        Task<bool> Delete(NotificationId id, CancellationToken cancellationToken);
    }
}