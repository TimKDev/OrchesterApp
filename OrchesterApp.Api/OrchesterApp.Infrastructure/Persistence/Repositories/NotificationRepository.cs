using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.ValueObjects;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace OrchesterApp.Infrastructure.Persistence.Repositories
{
    internal class NotificationRepository : INotificationRepository
    {
        private readonly OrchesterDbContext _context;

        public NotificationRepository(OrchesterDbContext context)
        {
            _context = context;
        }

        public Task<Notification> GetById(NotificationId id, CancellationToken cancellationToken)
        {
            return _context.Set<Notification>().FirstAsync(i => i.Id.Value == id.Value, cancellationToken);
        }

        public Task<List<Notification>> GetByIds(IList<NotificationId> ids, CancellationToken cancellationToken)
        {
            return _context.Set<Notification>().Where(n => ids.Contains(n.Id)).ToListAsync(cancellationToken);
        }

        public async Task<Notification> Save(Notification notification, CancellationToken cancellationToken)
        {
            await _context.Set<Notification>().AddAsync(notification, cancellationToken);
            return notification;
        }

        public async Task<bool> Delete(NotificationId id, CancellationToken cancellationToken)
        {
            var itemToRemove = await GetById(id, cancellationToken);
            _context.Set<Notification>().Remove(itemToRemove);

            return true;
        }
    }
}