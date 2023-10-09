using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.TerminRückmeldung.Models;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Features.TerminRückmeldung.Endpoints
{
    public static class GetRückmeldungTermin
    {
        public static void MapGetRückmeldungTerminEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/termin/rückmeldung/{terminId}", GetRückmeldungTerminById)
                .RequireAuthorization();
        }

        private static async Task<IResult> GetRückmeldungTerminById(Guid terminId, ISender sender, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetRückmeldungenTerminQuery(terminId), cancellationToken);

            return Results.Ok(result);
        }

        private record GetRückmeldungenTerminQuery(Guid TerminId) : IRequest<TerminRückmeldungsResponse>;

        private class GetRückmeldungenTerminQueryHandler : IRequestHandler<GetRückmeldungenTerminQuery, TerminRückmeldungsResponse>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

            public GetRückmeldungenTerminQueryHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                this.terminRepository = terminRepository;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<TerminRückmeldungsResponse> Handle(GetRückmeldungenTerminQuery request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                var terminRückmeldungOrchestermitglieder = new List<(string Vorname, string Nachname, string? VornameOther, string? NachnameOther, TerminRückmeldungOrchestermitglied TerminRückmeldungOrchestermitglied)>();
                foreach (var terminRückmeldung in termin.TerminRückmeldungOrchesterMitglieder)
                {
                    var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(terminRückmeldung.OrchesterMitgliedsId, cancellationToken);
                    if (terminRückmeldung.RückmeldungDurchAnderesOrchestermitglied is not null)
                    {
                        var otherOrchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(terminRückmeldung.RückmeldungDurchAnderesOrchestermitglied, cancellationToken);
                        terminRückmeldungOrchestermitglieder.Add(new(orchesterMitglied.Vorname, orchesterMitglied.Nachname, otherOrchesterMitglied.Vorname, otherOrchesterMitglied.Nachname, terminRückmeldung));

                    }
                    else
                    {
                        terminRückmeldungOrchestermitglieder.Add(new(orchesterMitglied.Vorname, orchesterMitglied.Nachname, null, null, terminRückmeldung));
                    }
                }

                return new TerminRückmeldungsResponse(termin.Id.Value, termin.Name, terminRückmeldungOrchestermitglieder.ToArray());
            }
        }

    }
}
