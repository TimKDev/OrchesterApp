using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using OrchesterApp.Domain.TerminAggregate.Entities;
using OrchesterApp.Domain.Common.ValueObjects;
using OrchesterApp.Domain.TerminAggregate;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Services;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;

namespace TvJahnOrchesterApp.Application.Features.Termin.Endpoints
{
    public static class UpdateTermin
    {
        public static void MapUpdateTerminEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/termin/update", GetTerminById)
                .RequireAuthorization(r => r.RequireRole(RoleNames.Admin, RoleNames.Vorstand));
        }

        private static async Task<IResult> GetTerminById(UpdateTerminCommand updateTerminCommand, ISender sender,
            CancellationToken cancellationToken)
        {
            var response = await sender.Send(updateTerminCommand, cancellationToken);
            return Results.Ok(response);
        }

        private record UpdateTerminCommand(
            Guid TerminId,
            string TerminName,
            int TerminArt,
            int TerminStatus,
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
            string? Image,
            string[] Dokumente) : IRequest<OrchesterApp.Domain.TerminAggregate.Termin>;

        private class
            UpdateTerminCommandHandler : IRequestHandler<UpdateTerminCommand,
            OrchesterApp.Domain.TerminAggregate.Termin>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IUnitOfWork unitOfWork;
            private readonly IFileStorageService _fileStorageService;

            public UpdateTerminCommandHandler(ITerminRepository terminRepository,
                IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork,
                IFileStorageService fileStorageService)
            {
                this.terminRepository = terminRepository;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.unitOfWork = unitOfWork;
                _fileStorageService = fileStorageService;
            }

            public async Task<OrchesterApp.Domain.TerminAggregate.Termin> Handle(UpdateTerminCommand request,
                CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken) ??
                             throw new Exception($"Termin {request.TerminId} not found");

                termin.UpdateName(request.TerminName);
                termin.UpdateTerminArt(request.TerminArt);
                termin.UpdateTerminStatus(request.TerminStatus);
                var imageCompressed = TransformImageService.ConvertToCompressedByteArray(request.Image);
                termin.UpdateImage(imageCompressed);

                var dokumenteForCleanup =
                    termin.UpdateDokumenteAndReturnDokumenteForCleanup(request.Dokumente
                        .Select(name => new TerminDokument(name)).ToList());

                var adresse = Adresse.Create(request.Straße, request.Hausnummer, request.Postleitzahl, request.Stadt,
                    request.Zusatz, request.Latitude, request.Longitude);
                termin.EinsatzPlan.UpdateEinsatzPlan(request.StartZeit, request.EndZeit, adresse,
                    request.WeitereInformationen);

                termin.EinsatzPlan.UpdateEinsatzplanNotenMappings(request.Noten.Select(EinsatzplanNotenMapping.Create));
                termin.EinsatzPlan.UpdateEinsatzplanUniformMappings(
                    request.Uniform.Select(EinsatzplanUniformMapping.Create));

                if (dokumenteForCleanup.Any())
                {
                    var deletionTasks = dokumenteForCleanup.Select(dokument =>
                        _fileStorageService.DeleteFileAsync(dokument.Name, cancellationToken));

                    await Task.WhenAll(deletionTasks);
                }

                if (request.OrchestermitgliedIds is null)
                {
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    return termin;
                }

                var orchesterMitglieder = await orchesterMitgliedRepository.QueryByIdAsync(
                    request.OrchestermitgliedIds.Select(OrchesterMitgliedsId.Create).ToArray(), cancellationToken);

                var terminRückmeldungOrchesterMitglieder = orchesterMitglieder.Select(o =>
                    TerminRückmeldungOrchestermitglied.Create(o.Id, new List<int?> { o.DefaultInstrument },
                        new List<int?> { o.DefaultNotenStimme })).ToArray();

                termin.UpdateTerminRückmeldungOrchestermitglied(terminRückmeldungOrchesterMitglieder);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return termin;
            }
        }
    }
}