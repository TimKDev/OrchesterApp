using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using TvJahnOrchesterApp.Application.Common.Interfaces.Dto;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.TerminAggregate;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

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
            var itemToRemove = await GetById(guid, cancellationToken);
            _context.Set<Termin>().Remove(itemToRemove);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<Termin[]> GetAll(CancellationToken cancellationToken)
        {
            return _context.Set<Termin>().ToArrayAsync(cancellationToken);
        }

        public Task<TerminWithResponses[]> GetTerminResponsesInYearAndPast(int year, CancellationToken cancellationToken)
        {
            return _context.Set<Termin>().AsNoTracking().Where(t => t.EinsatzPlan.EndZeit.Year == year && t.EinsatzPlan.StartZeit < DateTime.UtcNow).Select(t => new TerminWithResponses(t.Id, t.TerminArt, t.TerminRückmeldungOrchesterMitglieder)).ToArrayAsync(cancellationToken);
        }

        public async Task<Termin> GetById(Guid guid, CancellationToken cancellationToken)
        {
            return (await _context.Set<Termin>().ToListAsync(cancellationToken)).First(i => i.Id.Value == guid);
        }

        public async Task<Termin> Save(Termin termin, CancellationToken cancellationToken)
        {
            _context.Set<Termin>().Add(termin);
            await _context.SaveChangesAsync(cancellationToken);
            return termin;
        }
    }

    
}
