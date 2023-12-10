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
    internal class RückmeldungsArtenRepository : BaseDropdownRepository<Rückmeldungsart>, IDropdownRepository
    {
        public RückmeldungsArtenRepository(OrchesterDbContext context) : base(context)
        {
        }
    }
}
