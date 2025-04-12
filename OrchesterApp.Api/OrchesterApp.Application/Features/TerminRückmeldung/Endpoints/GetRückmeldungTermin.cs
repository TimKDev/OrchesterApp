using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;
using TvJahnOrchesterApp.Application.Features.Dropdown.Services;
using OrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Features.TerminRückmeldung.Endpoints
{
    public static class GetRückmeldungTermin
    {
        public static void MapGetRückmeldungTerminEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/termin/rückmeldung/{terminId}", GetRückmeldungTerminById)
                .RequireAuthorization(r => r.RequireRole(new string[] { RoleNames.Admin, RoleNames.Vorstand }));
        }

        private static async Task<IResult> GetRückmeldungTerminById(Guid terminId, ISender sender, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetRückmeldungenTerminQuery(terminId), cancellationToken);

            return Results.Ok(result);
        }

        public record TerminRückmeldungsTableEntry(Guid OrchesterMitgliedsId, string Vorname, string Nachname, string? VornameOther, string? NachnameOther, int Zugesagt, string? KommentarZusage, DateTime? LetzteRückmeldung, bool IstAnwesend, string? KommentarAnwesenheit);

        public record TerminRückmeldungsResponse(Guid TerminId, string TerminName, DateTime StartZeit, TerminRückmeldungsTableEntry[] TerminRückmeldungsTableEntries, DropdownItem[] ResponseDropdownValues);

        private record GetRückmeldungenTerminQuery(Guid TerminId) : IRequest<TerminRückmeldungsResponse>;

        private class GetRückmeldungenTerminQueryHandler : IRequestHandler<GetRückmeldungenTerminQuery, TerminRückmeldungsResponse>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IDropdownService dropdownService;

            public GetRückmeldungenTerminQueryHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository, IDropdownService dropdownService)
            {
                this.terminRepository = terminRepository;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.dropdownService = dropdownService;
            }

            public async Task<TerminRückmeldungsResponse> Handle(GetRückmeldungenTerminQuery request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                var terminRückmeldungOrchestermitglieder = new List<TerminRückmeldungsTableEntry>();
                var responseDropdownValues = await dropdownService.GetAllDropdownValuesAsync(DropdownNames.Rückmeldungsart, cancellationToken);

                foreach (var terminRückmeldung in termin.TerminRückmeldungOrchesterMitglieder)
                {
                    var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(terminRückmeldung.OrchesterMitgliedsId, cancellationToken);
                    OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied? otherOrchesterMitglied = null;
                    if (terminRückmeldung.RückmeldungDurchAnderesOrchestermitglied is not null)
                    {
                        otherOrchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(terminRückmeldung.RückmeldungDurchAnderesOrchestermitglied, cancellationToken);
                    }
                    terminRückmeldungOrchestermitglieder.Add(new TerminRückmeldungsTableEntry(orchesterMitglied.Id.Value, orchesterMitglied.Vorname, orchesterMitglied.Nachname, otherOrchesterMitglied?.Vorname, otherOrchesterMitglied?.Nachname, terminRückmeldung.Zugesagt, terminRückmeldung.KommentarZusage, terminRückmeldung.LetzteRückmeldung, terminRückmeldung.IstAnwesend, terminRückmeldung.KommentarAnwesenheit));
                }

                return new TerminRückmeldungsResponse(termin.Id.Value, termin.Name, termin.EinsatzPlan.StartZeit, terminRückmeldungOrchestermitglieder.OrderByDescending(e => e.Zugesagt).ToArray(), responseDropdownValues);
            }
        }

    }
}
