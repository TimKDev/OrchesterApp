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

        public DropdownService(IEnumerable<IDropdownRepository> dropdownRepositories)
        {
            this.dropdownRepositories = dropdownRepositories;
        }

        public Task<DropdownItem[]> GetAllDropdownValuesAsync(DropdownNames dropdownNames, CancellationToken cancellationToken)
        {
            var repo = dropdownRepositories.First(d => d.DropdownName == dropdownNames);
            return repo.GetAllDropdownItemsAsync(cancellationToken);
        }
    }
}
