using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;
using TvJahnOrchesterApp.Domain.Common.Entities;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Repositories.DropdownRepositories
{
    internal class TerminStatusRepository : BaseDropdownRepository<TerminStatus>, IDropdownRepository
    {
        public TerminStatusRepository(OrchesterDbContext context) : base(context)
        {
        }
    }
}
