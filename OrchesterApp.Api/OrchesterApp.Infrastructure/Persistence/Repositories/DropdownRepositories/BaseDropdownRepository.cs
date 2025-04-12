using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.Common.Interfaces;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;

namespace OrchesterApp.Infrastructure.Persistence.Repositories.DropdownRepositories
{
    internal abstract class BaseDropdownRepository<TDropdown> where TDropdown : class, IDropdownEntity
    {
        private readonly OrchesterDbContext _context;

        public BaseDropdownRepository(OrchesterDbContext context)
        {
            _context = context;
        }

        public DropdownNames DropdownName => (DropdownNames)Enum.Parse(typeof(DropdownNames), typeof(TDropdown).Name);

        public Task<DropdownItem[]> GetAllDropdownItemsAsync(CancellationToken cancellationToken)
        {
            return _context.Set<TDropdown>().Select(r => new DropdownItem(r.Id, r.Value)).ToArrayAsync(cancellationToken);
        }
    }
}
