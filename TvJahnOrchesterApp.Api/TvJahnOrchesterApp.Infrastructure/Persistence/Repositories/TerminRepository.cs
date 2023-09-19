using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.TerminAggregate;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Repositories
{
    internal class TerminRepository : ITerminRepository
    {
        private readonly OrchesterDbContext _context;

        public TerminRepository(OrchesterDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(Guid guid, CancellationToken cancellationToken)
        {
            var itemToRemove = _context.Set<Termin>().First(i => i.Id.Value == guid);
            _context.Set<Termin>().Remove(itemToRemove);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<Termin[]> GetAll(CancellationToken cancellationToken)
        {
            return _context.Set<Termin>().ToArrayAsync(cancellationToken);
        }

        public Task<Termin> GetById(Guid guid, CancellationToken cancellationToken)
        {
            return _context.Set<Termin>().FirstAsync(i => i.Id.Value == guid);
        }

        public async Task<Termin> Save(Termin termin, CancellationToken cancellationToken)
        {
            _context.Set<Termin>().Add(termin);
            await _context.SaveChangesAsync(cancellationToken);
            return termin;
        }
    }
}
