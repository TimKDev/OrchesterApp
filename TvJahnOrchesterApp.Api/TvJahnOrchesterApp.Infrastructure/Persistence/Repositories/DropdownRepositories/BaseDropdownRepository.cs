using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;
using TvJahnOrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Domain.Common.Interfaces;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Repositories.DropdownRepositories
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
