using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;

namespace TvJahnOrchesterApp.Application.Features.Dropdown.Services
{
    internal interface IDropdownService
    {
        Task<DropdownItem[]> GetAllDropdownValuesAsync(DropdownNames dropdownNames, CancellationToken cancellationToken);
    }
}
