using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.Common.Entities;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Repositories
{
    internal class InstrumentRepository: IInstrumentRepository
    {
        private readonly OrchesterDbContext _context;

        public InstrumentRepository(OrchesterDbContext context)
        {
            _context = context;
        }

        public Task<Instrument> GetByIdAsync(int Id, CancellationToken cancellationToken)
        {
            return _context.Set<Instrument>().FirstAsync(i => i.Id == Id, cancellationToken);
        }
    }
}
