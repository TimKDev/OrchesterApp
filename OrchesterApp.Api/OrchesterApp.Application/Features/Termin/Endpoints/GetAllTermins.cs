using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Dropdown.Enums;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;
using TvJahnOrchesterApp.Application.Features.Dropdown.Services;
using OrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Services;

namespace TvJahnOrchesterApp.Application.Features.Termin.Endpoints
{
    public static class GetAllTermins
    {
        public static void MapGetAllTerminsEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/termin/all", GetGetAllTermins)
                .RequireAuthorization();
        }

        private static async Task<IResult> GetGetAllTermins(ISender sender, CancellationToken cancellationToken)
        {
            var response = await sender.Send(new GetAllTermineQuery(), cancellationToken);
            return Results.Ok(response);
        }

        private record GetAllTermineQuery() : IRequest<GetAllTermineResponse>;

        private record GetAllTermineResponse(
            TerminData[] TerminData,
            DropdownItem[] TerminArtenDropdownValues,
            DropdownItem[] TerminStatusDropdownValues,
            DropdownItem[] ResponseDropdownValues);

        private record TerminData(
            Guid TerminId,
            string Name,
            int? TerminArt,
            int? TerminStatus,
            DateTime StartZeit,
            DateTime EndZeit,
            int Zugesagt,
            bool IstAnwesend,
            int NoResponse,
            int PositiveResponse,
            int NegativeResponse,
            string? Image,
            DateTime? FristAsDate,
            DateTime? ErsteWarnungVorFristAsDate);

        private class GetAllTermineQueryHandler : IRequestHandler<GetAllTermineQuery, GetAllTermineResponse>
        {
            private readonly ITerminRepository terminRepository;
            private readonly ICurrentUserService currentUserService;
            private readonly IDropdownService dropdownService;

            public GetAllTermineQueryHandler(ITerminRepository terminRepository, ICurrentUserService currentUserService,
                IDropdownService dropdownService)
            {
                this.terminRepository = terminRepository;
                this.currentUserService = currentUserService;
                this.dropdownService = dropdownService;
            }

            public async Task<GetAllTermineResponse> Handle(GetAllTermineQuery request,
                CancellationToken cancellationToken)
            {
                var terminResult = await GetTerminDataList(cancellationToken);
                var terminArtenDropdownValues =
                    await dropdownService.GetAllDropdownValuesAsync(DropdownNames.TerminArten, cancellationToken);
                var terminStatusDropdownValues =
                    await dropdownService.GetAllDropdownValuesAsync(DropdownNames.TerminStatus, cancellationToken);
                var responseDropdownValues =
                    await dropdownService.GetAllDropdownValuesAsync(DropdownNames.Rückmeldungsart, cancellationToken);

                return new GetAllTermineResponse(terminResult, terminArtenDropdownValues, terminStatusDropdownValues,
                    responseDropdownValues);
            }

            private async Task<TerminData[]> GetTerminDataList(CancellationToken cancellationToken)
            {
                var terminResult = new List<TerminData>();
                var termins = (await terminRepository.GetAll(cancellationToken));
                foreach (var termin in termins)
                {
                    var currentOrchesterMitglied =
                        await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
                    var currrentUserRückmeldung =
                        termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(r =>
                            r.OrchesterMitgliedsId == currentOrchesterMitglied.Id);

                    var countNoResponse = 0;
                    var countPositiveResponse = 0;
                    var countNegativeResponse = 0;
                    foreach (var rückmeldung in termin.TerminRückmeldungOrchesterMitglieder)
                    {
                        if (rückmeldung.Zugesagt == (int)RückmeldungsartEnum.NichtZurückgemeldet)
                        {
                            countNoResponse++;
                        }

                        if (rückmeldung.Zugesagt == (int)RückmeldungsartEnum.Zugesagt)
                        {
                            countPositiveResponse++;
                        }

                        if (rückmeldung.Zugesagt == (int)RückmeldungsartEnum.Abgesagt)
                        {
                            countNegativeResponse++;
                        }
                    }

                    var terminEntry = new TerminData(termin.Id.Value, termin.Name, termin.TerminArt,
                        termin.TerminStatus, termin.EinsatzPlan.StartZeit, termin.EinsatzPlan.EndZeit,
                        currrentUserRückmeldung?.Zugesagt ?? (int)RückmeldungsartEnum.NichtZurückgemeldet,
                        currrentUserRückmeldung?.IstAnwesend ?? false, countNoResponse, countPositiveResponse,
                        countNegativeResponse, TransformImageService.ConvertByteArrayToBase64(termin.Image),
                        termin.GetDeadlineDateTime(), termin.GetWarningDateTime());

                    terminResult.Add(terminEntry);
                }

                return terminResult.OrderBy(r => r.StartZeit).ToArray();
            }
        }
    }
}