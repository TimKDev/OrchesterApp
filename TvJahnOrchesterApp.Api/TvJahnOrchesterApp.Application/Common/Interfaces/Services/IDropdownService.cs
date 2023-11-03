using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Enums;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Services
{
    internal interface IDropdownService
    {
        DropdownItem[] GetAllDropdownValuesAsync(DropdownNames dropdownNames, CancellationToken cancellationToken);
    }
}
