using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.TerminAggregate;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Repositories
{
    internal class TerminRepository : ITerminRepository
    {
        private static List<Termin> persistedTermins = new List<Termin>();
        public Task<bool> Delete(Guid guid, CancellationToken cancellationToken)
        {
            var itemToRemove = persistedTermins.First(i => i.Id.Value == guid);
            persistedTermins.Remove(itemToRemove);
            return Task.FromResult(true);
        }

        public Task<Termin[]> GetAll(CancellationToken cancellationToken)
        {
            return Task.FromResult(persistedTermins.ToArray());
        }

        public Task<Termin> GetById(Guid guid, CancellationToken cancellationToken)
        {
            return Task.FromResult(persistedTermins.First(i => i.Id.Value == guid));
        }

        public Task<Termin> Save(Termin termin, CancellationToken cancellationToken)
        {
            persistedTermins.Add(termin);
            return Task.FromResult(termin);
        }
    }
}
