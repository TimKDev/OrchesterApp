using OrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace OrchesterApp.Infrastructure.Persistence.Repositories.DropdownRepositories
{
    internal class TerminStatusRepository : BaseDropdownRepository<TerminStatus>, IDropdownRepository
    {
        public TerminStatusRepository(OrchesterDbContext context) : base(context)
        {
        }
    }
}
