using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories
{
    public interface IDropdownRepository
    {
        DropdownNames DropdownName { get; }
        Task<DropdownItem[]> GetAllDropdownItemsAsync(CancellationToken cancellationToken);
    }
}
