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
    internal static class GetOrchesterMitgliederDropdown
    {
        public static void MapGetOrchesterMitgliederDropdownEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/dropdown/orchester-mitglieder", GetDropdownValues)
                .CacheOutput("OutputCacheWithAuthPolicy")
                .RequireAuthorization();
        }

        private static async Task<OrchesterMitgliederDropdownResponse[]> GetDropdownValues(CancellationToken cancellationToken, ISender sender)
        {
            return await sender.Send(new GetOrchesterMitgliederDropdownQuery());
        }

        private record GetOrchesterMitgliederDropdownQuery() : IRequest<OrchesterMitgliederDropdownResponse[]>;

        private record OrchesterMitgliederDropdownResponse(Guid Value, string Text);

        private class GetDropdownQueryHandler : IRequestHandler<GetOrchesterMitgliederDropdownQuery, OrchesterMitgliederDropdownResponse[]>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

            public GetDropdownQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<OrchesterMitgliederDropdownResponse[]> Handle(GetOrchesterMitgliederDropdownQuery request, CancellationToken cancellationToken)
            {
                return (await orchesterMitgliedRepository.GetAllAsync(cancellationToken)).Select(m => new OrchesterMitgliederDropdownResponse(m.Id.Value, $"{m.Vorname} {m.Nachname}")).ToArray();
            }
        }
    }
}
