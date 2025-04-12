using OrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace OrchesterApp.Infrastructure.Persistence.Repositories.DropdownRepositories
{
    internal class RückmeldungsArtenRepository : BaseDropdownRepository<Rückmeldungsart>, IDropdownRepository
    {
        public RückmeldungsArtenRepository(OrchesterDbContext context) : base(context)
        {
        }
    }
}
