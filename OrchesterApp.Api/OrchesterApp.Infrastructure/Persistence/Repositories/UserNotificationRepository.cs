using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.UserAggregate.ValueObjects;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace OrchesterApp.Infrastructure.Persistence.Repositories
{
    internal class UserNotificationRepository : IUserNotificationRepository
    {
        private readonly OrchesterDbContext _context;

        public UserNotificationRepository(OrchesterDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(UserNotificationId id, CancellationToken cancellationToken)
        {
            var itemToRemove = await GetById(id, cancellationToken);
            _context.Set<UserNotification>().Remove(itemToRemove);

            return true;
        }

        public Task<UserNotification> GetById(UserNotificationId id, CancellationToken cancellationToken)
        {
            return _context.Set<UserNotification>().FirstAsync(i => i.Id.Value == id.Value, cancellationToken);
        }

        public Task<List<UserNotification>> GetByIds(List<UserNotificationId> ids, CancellationToken cancellationToken)
        {
            return _context.Set<UserNotification>().Where(u => ids.Contains(u.Id)).ToListAsync(cancellationToken);
        }

        public Task<UserNotification[]> GetByUserId(UserId userId, int limitResult, CancellationToken cancellationToken)
        {
            return _context.Set<UserNotification>()
                .Where(un => un.UserId == userId && un.NotificationSink == NotificationSink.Portal)
                .OrderBy(un => un.CreatedAt)
                .Take(limitResult)
                .ToArrayAsync(cancellationToken);
        }

        public Task<UserNotification[]> GetByNotificationIds(IList<NotificationId> notificationIds,
            CancellationToken cancellationToken)
        {
            return _context.Set<UserNotification>()
                .Where(un => notificationIds.Contains(un.NotificationId))
                .ToArrayAsync(cancellationToken);
        }

        public Task Save(IList<UserNotification> userNotification,
            CancellationToken cancellationToken)
        {
            return _context.Set<UserNotification>().AddRangeAsync(userNotification, cancellationToken);
        }
    }
}