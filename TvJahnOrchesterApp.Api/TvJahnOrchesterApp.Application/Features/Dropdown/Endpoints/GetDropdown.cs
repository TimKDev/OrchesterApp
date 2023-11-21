using MediatR;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using TvJahnOrchesterApp.Application.Features.Dropdown.Services;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace TvJahnOrchesterApp.Application.Features.Dropdown.Endpoints
{
    internal static class GetDropdown
    {
        public static void MapGetDropdownEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/dropdown/{dropdownName}", GetDropdownValues)
                .CacheOutput("OutputCacheWithAuthPolicy")
                .RequireAuthorization();
        }

        private static async Task<DropdownItem[]> GetDropdownValues(string dropdownName, CancellationToken cancellationToken, ISender sender)
        {
            return await sender.Send(new GetDropdownQuery(dropdownName));
        }

        private record GetDropdownQuery(string DropdownName) : IRequest<DropdownItem[]>;

        private class GetDropdownQueryHandler : IRequestHandler<GetDropdownQuery, DropdownItem[]>
        {
            private readonly IDropdownService dropdownService;
            public GetDropdownQueryHandler(IDropdownService dropdownService)
            {
                this.dropdownService = dropdownService;
            }

            public async Task<DropdownItem[]> Handle(GetDropdownQuery request, CancellationToken cancellationToken)
            {
                DropdownNames dropdownName = (DropdownNames)Enum.Parse(typeof(DropdownNames), request.DropdownName);
                var result = (await dropdownService.GetAllDropdownValuesAsync(dropdownName, cancellationToken)).ToList();
                result.Add(new DropdownItem(null, "Nichts ausgewählt"));

                return result.ToArray();
            }
        }
    }
}
