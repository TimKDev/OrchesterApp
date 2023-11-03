using MediatR;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TvJahnOrchesterApp.Application.Features.TerminDashboard.GetNextTermins;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using TvJahnOrchesterApp.Application.Common.Models;
using TvJahnOrchesterApp.Application.Common.Enums;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;

namespace TvJahnOrchesterApp.Application.Common.Endpoints
{
    internal class GetDropdown
    {
        public static void MapGetDropdownEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/dropdown/{dropdownName}", GetDropdownValues)
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
                return await dropdownService.GetAllDropdownValuesAsync(request.DropdownName, cancellationToken);
            }
        }
    }
}
