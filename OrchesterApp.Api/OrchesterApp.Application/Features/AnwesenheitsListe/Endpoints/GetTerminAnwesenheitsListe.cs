﻿using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.AnwesenheitsListe.Models;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;

namespace TvJahnOrchesterApp.Application.Features.AnwesenheitsListe.Endpoints
{
    public static class GetTerminAnwesenheitsListe
    {
        public static void MapGetTerminAnwesenheitsListeEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/termin/anwesenheit/{terminId}", GetSpecificTerminAnwesenheitsListe)
                .RequireAuthorization(r => r.RequireRole(new string[] { RoleNames.Admin, RoleNames.Vorstand }));
        }

        private static async Task<IResult> GetSpecificTerminAnwesenheitsListe(Guid terminId, ISender sender, CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetAnwesenheitsListeQuery(terminId), cancellationToken);
            return Results.Ok(response);
        }

        private record GetAnwesenheitsListeQuery(Guid TerminId) : IRequest<TerminAnwesenheitsListenEintrag[]>;

        private class GetAnwesenheitsListeQueryHandler : IRequestHandler<GetAnwesenheitsListeQuery, TerminAnwesenheitsListenEintrag[]>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly ITerminRepository terminRepository;

            public GetAnwesenheitsListeQueryHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                this.terminRepository = terminRepository;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<TerminAnwesenheitsListenEintrag[]> Handle(GetAnwesenheitsListeQuery request, CancellationToken cancellationToken)
            {
                var result = new List<TerminAnwesenheitsListenEintrag>();
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                foreach (OrchesterApp.Domain.TerminAggregate.Entities.TerminRückmeldungOrchestermitglied rückmeldung in termin.TerminRückmeldungOrchesterMitglieder)
                {
                    var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(rückmeldung.OrchesterMitgliedsId, cancellationToken);
                    result.Add(new TerminAnwesenheitsListenEintrag(orchesterMitglied.Vorname, orchesterMitglied.Nachname, rückmeldung));
                }
                return result.ToArray();
            }
        }
    }
}
