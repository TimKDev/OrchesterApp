using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;

namespace OrchesterApp.Infrastructure.Persistence.Repositories.DropdownRepositories
{
    internal class UniformRepository : IDropdownRepository
    {
        private readonly OrchesterDbContext _context;

        public UniformRepository(OrchesterDbContext context)
        {
            _context = context;
        }

        public DropdownNames DropdownName => DropdownNames.Uniform;

        public Task<DropdownItem[]> GetAllDropdownItemsAsync(CancellationToken cancellationToken)
        {
            return _context.Set<Uniform>().Select(x => new DropdownItem(x.Id, x.Value)).ToArrayAsync(cancellationToken);
        }
    }
}
