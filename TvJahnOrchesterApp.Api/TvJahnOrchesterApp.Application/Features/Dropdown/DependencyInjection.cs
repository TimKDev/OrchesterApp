using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Features.Authorization.Interfaces;
using TvJahnOrchesterApp.Application.Features.Authorization.Services;
using TvJahnOrchesterApp.Application.Features.Dropdown.Services;

namespace TvJahnOrchesterApp.Application.Features.Dropdown
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDashboardFeature(this IServiceCollection services)
        {
            return services.AddScoped<IDropdownService, DropdownService>();
        }
    }
}
