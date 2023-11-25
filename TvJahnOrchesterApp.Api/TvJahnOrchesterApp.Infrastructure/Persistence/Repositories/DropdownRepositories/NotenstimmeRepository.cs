using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;
using TvJahnOrchesterApp.Domain.Common.Entities;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Repositories.DropdownRepositories
{
    internal class NotenstimmeRepository : IDropdownRepository
    {
        private readonly OrchesterDbContext _context;

        public NotenstimmeRepository(OrchesterDbContext context)
        {
            _context = context;
        }

        public DropdownNames DropdownName => DropdownNames.Notenstimme;

        public Task<DropdownItem[]> GetAllDropdownItemsAsync(CancellationToken cancellationToken)
        {
            return _context.Set<Notenstimme>().Select(x => new DropdownItem(x.Id, x.Value)).ToArrayAsync(cancellationToken);
        }
    }
}
