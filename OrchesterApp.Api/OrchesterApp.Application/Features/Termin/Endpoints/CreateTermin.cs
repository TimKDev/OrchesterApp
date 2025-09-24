using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Services;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using OrchesterApp.Domain.Common.ValueObjects;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using OrchesterApp.Domain.TerminAggregate.Entities;
using static TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints.CreateOrchesterMitglied;

namespace TvJahnOrchesterApp.Application.Features.Termin.Endpoints
{
    public static class TerminCreate
    {
        public static void MapTerminCreateEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/termin/create", PostTerminCreate)
                .RequireAuthorization(r => r.RequireRole(new string[] { RoleNames.Admin, RoleNames.Vorstand }));
        }

        private static async Task<IResult> PostTerminCreate([FromBody] CreateTerminCommand createTerminCommand,
            ISender sender, CancellationToken cancellationToken)
        {
            var result = await sender.Send(createTerminCommand, cancellationToken);

            return Results.Ok(result);
        }

        private record CreateTerminCommand(
            string Name,
            int? TerminArt,
            DateTime StartZeit,
            DateTime EndZeit,
            string Straße,
            string Hausnummer,
            string Postleitzahl,
            string Stadt,
            string? Zusatz,
            decimal? Latitude,
            decimal? Longitude,
            int[] Noten,
            int[] Uniform,
            Guid[]? OrchestermitgliedIds,
            string? WeitereInformationen,
            string? Image) : IRequest<OrchesterApp.Domain.TerminAggregate.Termin>;

        private class
            CreateTerminCommandHandler : IRequestHandler<CreateTerminCommand,
            OrchesterApp.Domain.TerminAggregate.Termin>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

            public CreateTerminCommandHandler(ITerminRepository terminRepository,
                IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                this.terminRepository = terminRepository;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<OrchesterApp.Domain.TerminAggregate.Termin> Handle(CreateTerminCommand request,
                CancellationToken cancellationToken)
            {
                OrchesterApp.Domain.OrchesterMitgliedAggregate.OrchesterMitglied[] orchesterMitglieder;
                if (request.OrchestermitgliedIds is not null && request.OrchestermitgliedIds.Length > 0)
                {
                    orchesterMitglieder = await orchesterMitgliedRepository.QueryByIdsAsync(
                        request.OrchestermitgliedIds.Select(OrchesterMitgliedsId.Create).ToArray(), cancellationToken);
                }
                else
                {
                    orchesterMitglieder = await orchesterMitgliedRepository.GetAllAsync(cancellationToken);
                }

                var terminRückmeldungOrchesterMitglieder = orchesterMitglieder.Select(o =>
                    TerminRückmeldungOrchestermitglied.Create(o.Id, new List<int?> { o.DefaultInstrument },
                        new List<int?> { o.DefaultNotenStimme })).ToArray();

                var treffpunkt = Adresse.Create(request.Straße, request.Hausnummer, request.Postleitzahl, request.Stadt,
                    request.Zusatz, request.Latitude, request.Longitude);

                var compressedImage = TransformImageService.ConvertToCompressedByteArray(request.Image);

                var termin = OrchesterApp.Domain.TerminAggregate.Termin.Create(terminRückmeldungOrchesterMitglieder,
                    request.Name, request.TerminArt, request.StartZeit, request.EndZeit, treffpunkt,
                    request.Noten?.ToList(), request.Uniform?.ToList(), zusätzlicheInfo: request.WeitereInformationen,
                    image: compressedImage);

                return await terminRepository.Save(termin, cancellationToken);
            }
        }
    }
}