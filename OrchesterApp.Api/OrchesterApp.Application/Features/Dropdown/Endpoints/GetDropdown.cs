using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using TvJahnOrchesterApp.Application.Features.Dropdown.Services;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using Microsoft.Extensions.DependencyInjection;

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

        private static async Task<DropdownItem[]> GetDropdownValues(string dropdownName,
            CancellationToken cancellationToken, ISender sender)
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
                var result = (await dropdownService.GetAllDropdownValuesAsync(dropdownName, cancellationToken))
                    .ToList();
                result.Add(new DropdownItem(null, "Nichts ausgewählt"));

                return result.ToArray();
            }
        }
    }
}