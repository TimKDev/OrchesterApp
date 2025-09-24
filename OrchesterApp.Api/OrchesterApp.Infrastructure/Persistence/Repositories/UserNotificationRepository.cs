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
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<UserNotification> GetById(UserNotificationId id, CancellationToken cancellationToken)
        {
            return (await _context.Set<UserNotification>().ToListAsync(cancellationToken)).First(i => i.Id.Value == id.Value);
        }

        public async Task<UserNotification[]> GetByUserId(UserId userId, CancellationToken cancellationToken)
        {
            return (await _context.Set<UserNotification>().ToListAsync(cancellationToken))
                .Where(un => un.UserId.Value == userId.Value).ToArray();
        }

        public async Task<UserNotification[]> GetByNotificationId(NotificationId notificationId, CancellationToken cancellationToken)
        {
            return (await _context.Set<UserNotification>().ToListAsync(cancellationToken))
                .Where(un => un.NotificationId.Value == notificationId.Value).ToArray();
        }

        public async Task<UserNotification> Save(UserNotification userNotification, CancellationToken cancellationToken)
        {
            await _context.Set<UserNotification>().AddAsync(userNotification, cancellationToken);
            return userNotification;
        }
    }
} 