using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Repositories
{
    internal class OrchesterMitgliedRepository : IOrchesterMitgliedRepository
    {
        private readonly OrchesterDbContext _context;

        public OrchesterMitgliedRepository(OrchesterDbContext context)
        {
            _context = context;
        }

        public async Task<OrchesterMitglied> CreateAsync(OrchesterMitglied orchesterMitglied, CancellationToken cancellationToken)
        {
            _context.Set<OrchesterMitglied>().Add(orchesterMitglied);   
            await _context.SaveChangesAsync(cancellationToken);
            return orchesterMitglied;
        }

        public async Task<OrchesterMitglied[]> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<OrchesterMitglied>().ToArrayAsync(cancellationToken);
        }

        public async Task<OrchesterMitglied> GetByIdAsync(OrchesterMitgliedsId id, CancellationToken cancellationToken)
        {
            return await _context.Set<OrchesterMitglied>().FirstAsync(m => m.Id == id);
        }

        public async Task<OrchesterMitglied?> GetByNameAsync(string vorname, string nachname, CancellationToken cancellationToken)
        {
            return await _context.Set<OrchesterMitglied>().FirstOrDefaultAsync(m => m.Vorname == vorname && m.Nachname == nachname);
        }

        public async Task<OrchesterMitglied?> GetByRegistrationKeyAsync(string registrationKey, CancellationToken cancellationToken)
        {
            return await _context.Set<OrchesterMitglied>().FirstOrDefaultAsync(m => m.RegisterKey == registrationKey);
        }

        public async Task<OrchesterMitglied[]> QueryByIdAsync(OrchesterMitgliedsId[] ids, CancellationToken cancellationToken)
        {
            return await _context.Set<OrchesterMitglied>().Where(o => ids.Contains(o.Id)).ToArrayAsync(cancellationToken);
        }
    }
}
