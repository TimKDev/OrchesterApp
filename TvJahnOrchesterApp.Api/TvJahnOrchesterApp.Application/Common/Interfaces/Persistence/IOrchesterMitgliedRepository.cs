namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence
{
    public interface IOrchesterMitgliedRepository
    {
        Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]> GetAllAsync(CancellationToken cancellationToken);
        Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied> CreateAsync(Domain.OrchesterMitgliedAggregate.OrchesterMitglied orchesterMitglied, CancellationToken cancellationToken);
        Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied?> GetByNameAsync(string vorname, string lastname, CancellationToken cancellationToken);
    }
}
