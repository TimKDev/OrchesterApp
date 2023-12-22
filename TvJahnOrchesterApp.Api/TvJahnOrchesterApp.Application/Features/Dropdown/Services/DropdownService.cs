using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;

namespace TvJahnOrchesterApp.Application.Features.Dropdown.Services
{
    internal class DropdownService : IDropdownService
    {
        private readonly IEnumerable<IDropdownRepository> dropdownRepositories;
        private readonly Dictionary<DropdownNames, DropdownItem[]> cachedDropdownValues = new Dictionary<DropdownNames, DropdownItem[]>();

        public DropdownService(IEnumerable<IDropdownRepository> dropdownRepositories)
        {
            this.dropdownRepositories = dropdownRepositories;
        }

        public async Task<DropdownItem[]> GetAllDropdownValuesAsync(DropdownNames dropdownNames, CancellationToken cancellationToken)
        {
            if(cachedDropdownValues.TryGetValue(dropdownNames, out DropdownItem[]? cachedResult) && cachedResult is not null)
            {
                return cachedResult;
            }
            var repo = dropdownRepositories.First(d => d.DropdownName == dropdownNames);
            var result = await repo.GetAllDropdownItemsAsync(cancellationToken);
            cachedDropdownValues.Add(dropdownNames, result);

            return result;
        }
    }
}
