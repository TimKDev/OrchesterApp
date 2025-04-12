using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;

namespace OrchesterApp.Infrastructure.Persistence
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly OrchesterDbContext _context;

        public UnitOfWork(OrchesterDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
