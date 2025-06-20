﻿using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;

namespace OrchesterApp.Infrastructure.Persistence.Repositories.DropdownRepositories
{
    internal class InstrumentRepository : IInstrumentRepository, IDropdownRepository
    {
        private readonly OrchesterDbContext _context;

        public InstrumentRepository(OrchesterDbContext context)
        {
            _context = context;
        }

        public DropdownNames DropdownName => DropdownNames.Instrument;

        public Task<DropdownItem[]> GetAllDropdownItemsAsync(CancellationToken cancellationToken)
        {
            return _context.Set<Instrument>().Select(x => new DropdownItem(x.Id, x.Value)).ToArrayAsync(cancellationToken);
        }

        public Task<Instrument> GetByIdAsync(int Id, CancellationToken cancellationToken)
        {
            return _context.Set<Instrument>().FirstAsync(i => i.Id == Id, cancellationToken);
        }
    }
}
