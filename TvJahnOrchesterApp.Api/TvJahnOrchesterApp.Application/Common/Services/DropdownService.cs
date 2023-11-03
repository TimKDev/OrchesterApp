using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Enums;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Common.Services
{
    internal class DropdownService: IDropdownService
    {
        public DropdownService(IEnumerable<IDropdownRepository> dropdownRepositories) { }

        public DropdownItem[] GetAllDropdownValuesAsync(DropdownNames dropdownNames, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
