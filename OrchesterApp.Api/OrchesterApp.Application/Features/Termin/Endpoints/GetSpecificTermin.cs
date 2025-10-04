using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Services;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;
using TvJahnOrchesterApp.Application.Features.Dropdown.Services;

namespace TvJahnOrchesterApp.Application.Features.Termin.Endpoints
{
    public static class GetSpecificTermin
    {
        public static void MapGetSpecificTerminEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/termin/getById/{id}", GetTerminById)
                .RequireAuthorization();
        }

        private static async Task<IResult> GetTerminById(Guid id, ISender sender, CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetTerminByIdQuery(id), cancellationToken);
            return Results.Ok(response);
        }

        private record GetTerminByIdQuery(Guid Id) : IRequest<GetTerminByIdResponse>;

        private record GetTerminByIdResponse(
            TerminDetails Termin,
            TerminRückmeldung? TerminRückmeldung,
            DropdownItem[] TerminArtenDropdownValues,
            DropdownItem[] TerminStatusDropdownValues,
            DropdownItem[] ResponseDropdownValues,
            DropdownItem[] NotenDropdownValues,
            DropdownItem[] UniformDropdownValues);

        private record TerminDetails(
            string TerminName,
            int? TerminArt,
            int? TerminStatus,
            DateTime StartZeit,
            DateTime EndZeit,
            string? Straße,
            string? Hausnummer,
            string? Postleitzahl,
            string? Stadt,
            string? Zusatz,
            decimal? Latitude,
            decimal? Longitude,
            int[] Noten,
            int[] Uniform,
            string? WeitereInformationen,
            string? Image,
            string[] Dokumente,
            TimeSpan? Frist,
            TimeSpan? ErsteWarnungVorFrist,
            DateTime? FristAsDate
            );

        private record TerminRückmeldung(
            int Zugesagt,
            string? KommentarZusage,
            string? VornameOther,
            string? NachnameOther,
            bool IstAnwesend,
            string? KommentarAnwesenheit);

        private class GetTerminByIdQueryHandler : IRequestHandler<GetTerminByIdQuery, GetTerminByIdResponse>
        {
            private readonly ITerminRepository terminRepository;
            private readonly ICurrentUserService currentUserService;
            private readonly IDropdownService dropdownService;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

            public GetTerminByIdQueryHandler(ITerminRepository terminRepository, ICurrentUserService currentUserService,
                IDropdownService dropdownService, IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                this.terminRepository = terminRepository;
                this.currentUserService = currentUserService;
                this.dropdownService = dropdownService;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<GetTerminByIdResponse> Handle(GetTerminByIdQuery request,
                CancellationToken cancellationToken)
            {
                var terminArtenDropdownValues =
                    await dropdownService.GetAllDropdownValuesAsync(DropdownNames.TerminArten, cancellationToken);
                var terminStatusDropdownValues =
                    await dropdownService.GetAllDropdownValuesAsync(DropdownNames.TerminStatus, cancellationToken);
                var responseDropdownValues =
                    await dropdownService.GetAllDropdownValuesAsync(DropdownNames.Rückmeldungsart, cancellationToken);
                var notenDropdownValues =
                    await dropdownService.GetAllDropdownValuesAsync(DropdownNames.Noten, cancellationToken);
                var uniformDropdownValues =
                    await dropdownService.GetAllDropdownValuesAsync(DropdownNames.Uniform, cancellationToken);

                var termin = await terminRepository.GetById(request.Id, cancellationToken);
                var currentOrchesterMitglied =
                    await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
                var currrentUserRueckmeldung =
                    termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(r =>
                        r.OrchesterMitgliedsId == currentOrchesterMitglied.Id);

                OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied? otherOrchesterMitglied = null;
                if (currrentUserRueckmeldung is not null &&
                    currrentUserRueckmeldung.RückmeldungDurchAnderesOrchestermitglied is not null)
                {
                    otherOrchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(
                        currrentUserRueckmeldung.RückmeldungDurchAnderesOrchestermitglied, cancellationToken);
                }

                return new GetTerminByIdResponse(
                    new TerminDetails(termin.Name, termin.TerminArt, termin.TerminStatus, termin.EinsatzPlan.StartZeit,
                        termin.EinsatzPlan.EndZeit, termin.EinsatzPlan.Treffpunkt.Straße,
                        termin.EinsatzPlan.Treffpunkt.Hausnummer, termin.EinsatzPlan.Treffpunkt.Postleitzahl,
                        termin.EinsatzPlan.Treffpunkt.Stadt, termin.EinsatzPlan.Treffpunkt.Zusatz,
                        termin.EinsatzPlan.Treffpunkt.Latitude, termin.EinsatzPlan.Treffpunkt.Longitide,
                        termin.EinsatzPlan.EinsatzplanNotenMappings.Select(n => n.NotenId).ToArray(),
                        termin.EinsatzPlan.EinsatzplanUniformMappings.Select(t => t.UniformId).ToArray(),
                        termin.EinsatzPlan.WeitereInformationen,
                        TransformImageService.ConvertByteArrayToBase64(termin.Image),
                        termin.Dokumente.Select(t => t.Name).ToArray(), termin.Frist, termin.ErsteWarnungVorFrist, termin.GetDeadlineDateTime()),
                    currrentUserRueckmeldung is null
                        ? null
                        : new TerminRückmeldung(currrentUserRueckmeldung.Zugesagt,
                            currrentUserRueckmeldung.KommentarZusage, otherOrchesterMitglied?.Vorname,
                            otherOrchesterMitglied?.Nachname, currrentUserRueckmeldung.IstAnwesend,
                            currrentUserRueckmeldung.KommentarAnwesenheit),
                    terminArtenDropdownValues,
                    terminStatusDropdownValues,
                    responseDropdownValues,
                    notenDropdownValues,
                    uniformDropdownValues
                );
            }
        }
    }
}