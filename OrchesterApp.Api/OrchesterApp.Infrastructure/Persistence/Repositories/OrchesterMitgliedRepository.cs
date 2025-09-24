using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.OrchesterMitgliedAggregate;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Application.Common.Interfaces.Dto;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace OrchesterApp.Infrastructure.Persistence.Repositories
{
    internal class OrchesterMitgliedRepository : IOrchesterMitgliedRepository
    {
        private readonly OrchesterDbContext _context;

        public OrchesterMitgliedRepository(OrchesterDbContext context)
        {
            _context = context;
        }

        public async Task<OrchesterMitglied> CreateAsync(OrchesterMitglied orchesterMitglied,
            CancellationToken cancellationToken)
        {
            _context.Set<OrchesterMitglied>().Add(orchesterMitglied);
            await _context.SaveChangesAsync(cancellationToken);
            return orchesterMitglied;
        }

        public Task<OrchesterMitglied[]> GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.Set<OrchesterMitglied>().ToArrayAsync(cancellationToken);
        }

        public Task<OrchesterMitgliedWithName[]> GetAllNames(CancellationToken cancellationToken)
        {
            return _context.Set<OrchesterMitglied>()
                .Select(o => new OrchesterMitgliedWithName(o.Id, o.Vorname, o.Nachname))
                .ToArrayAsync(cancellationToken);
        }

        public Task<OrchesterMitgliedAdminInfo[]> GetAllAdminInfo(CancellationToken cancellationToken)
        {
            return _context.Set<OrchesterMitglied>()
                .Select(o =>
                    new OrchesterMitgliedAdminInfo(o.Id, o.Vorname, o.Nachname, o.ConnectedUserId, o.UserLastLogin))
                .ToArrayAsync(cancellationToken);
        }

        public async Task<OrchesterMitglied> GetByIdAsync(OrchesterMitgliedsId id, CancellationToken cancellationToken)
        {
            return await _context.Set<OrchesterMitglied>().FirstAsync(m => m.Id == id);
        }

        public async Task<OrchesterMitglied?> GetByNameAsync(string vorname, string nachname,
            CancellationToken cancellationToken)
        {
            return await _context.Set<OrchesterMitglied>()
                .FirstOrDefaultAsync(m => m.Vorname == vorname && m.Nachname == nachname);
        }

        public async Task<OrchesterMitglied?> GetByRegistrationKeyAsync(string registrationKey,
            CancellationToken cancellationToken)
        {
            return await _context.Set<OrchesterMitglied>().FirstOrDefaultAsync(m => m.RegisterKey == registrationKey);
        }

        public async Task<OrchesterMitglied[]> QueryByIdsAsync(OrchesterMitgliedsId[] ids,
            CancellationToken cancellationToken)
        {
            return await _context.Set<OrchesterMitglied>().Where(o => ids.Contains(o.Id))
                .ToArrayAsync(cancellationToken);
        }

        public async Task DeleteByIdAsync(OrchesterMitgliedsId orchesterMitgliedsId,
            CancellationToken cancellationToken)
        {
            var orchesterMitglied = await GetByIdAsync(orchesterMitgliedsId, cancellationToken);
            _context.Remove(orchesterMitglied);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<OrchesterMitglied?> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.Set<OrchesterMitglied>().FirstOrDefaultAsync(m => m.ConnectedUserId == userId);
        }
    }
}