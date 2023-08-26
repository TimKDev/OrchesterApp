
using TvJahnOrchesterApp.Contracts.OrchestraMembers;

namespace TvJahnOrchesterApp.Application.Services
{
    public interface IOrchestraMemberService
    {
        Task CreateOrchestraMemberAsync(OrchestraMemberContract orchestraMemberContract, CancellationToken cancellationToken);
    }
}