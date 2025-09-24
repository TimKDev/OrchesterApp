using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.NotificationAggregate;
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

        public async Task<bool> Delete(NotificationId id, CancellationToken cancellationToken)
        {
            var itemToRemove = await GetById(id, cancellationToken);
            _context.Set<Notification>().Remove(itemToRemove);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Notification> GetById(NotificationId id, CancellationToken cancellationToken)
        {
            return (await _context.Set<Notification>().ToListAsync(cancellationToken)).First(i => i.Id.Value == id.Value);
        }

        public async Task<Notification> Save(Notification notification, CancellationToken cancellationToken)
        {
            await _context.Set<Notification>().AddAsync(notification, cancellationToken);
            return notification;
        }
    }
} 