using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.NotificationAggregate.ValueObjects;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> Save(Notification notification, CancellationToken cancellationToken);
        Task<Notification> GetById(NotificationId id, CancellationToken cancellationToken);
        Task<List<Notification>> GetByIds(IList<NotificationId> ids, CancellationToken cancellationToken);
        Task<bool> Delete(NotificationId id, CancellationToken cancellationToken);

        Task<Notification[]> QueryByTerminAndCategoryAsync(List<TerminId> terminIds, List<NotificationCategory> categories,
            CancellationToken cancellationToken);
    }
}