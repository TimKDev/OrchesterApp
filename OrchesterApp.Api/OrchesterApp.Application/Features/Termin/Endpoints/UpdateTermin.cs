using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using OrchesterApp.Domain.TerminAggregate.Entities;
using OrchesterApp.Domain.Common.ValueObjects;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Notifications;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;
using TvJahnOrchesterApp.Application.Common.Interfaces;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Services;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;

namespace TvJahnOrchesterApp.Application.Features.Termin.Endpoints
{
    public static class UpdateTermin
    {
        public static void MapUpdateTerminEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/termin/update", UpdateTerminById)
                .RequireAuthorization(r => r.RequireRole(RoleNames.Admin, RoleNames.Vorstand));
        }

        private static async Task<IResult> UpdateTerminById(UpdateTerminCommand updateTerminCommand, ISender sender,
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
            string[] Dokumente,
            bool ShouldEmailBeSend) : IRequest<OrchesterApp.Domain.TerminAggregate.Termin>;

        private class
            UpdateTerminCommandHandler : IRequestHandler<UpdateTerminCommand,
            OrchesterApp.Domain.TerminAggregate.Termin>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IUnitOfWork unitOfWork;
            private readonly IFileStorageService _fileStorageService;
            private readonly INotifyService _notifyService;
            private readonly INotificationBackgroundService _notificationBackgroundService;
            private readonly ICurrentUserService _currentUserService;

            public UpdateTerminCommandHandler(ITerminRepository terminRepository,
                IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork,
                IFileStorageService fileStorageService, INotifyService notifyService,
                INotificationBackgroundService notificationBackgroundService, ICurrentUserService currentUserService)
            {
                this.terminRepository = terminRepository;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.unitOfWork = unitOfWork;
                _fileStorageService = fileStorageService;
                _notifyService = notifyService;
                _notificationBackgroundService = notificationBackgroundService;
                _currentUserService = currentUserService;
            }

            public async Task<OrchesterApp.Domain.TerminAggregate.Termin> Handle(UpdateTerminCommand request,
                CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken) ??
                             throw new Exception($"Termin {request.TerminId} not found");

                var oldTerminData = new TerminData(
                    termin.TerminStatus,
                    termin.EinsatzPlan.StartZeit,
                    termin.EinsatzPlan.EndZeit,
                    termin.EinsatzPlan.Treffpunkt,
                    termin.Dokumente.Select(d => d.Name).ToList(),
                    termin.EinsatzPlan.EinsatzplanUniformMappings.Select(u => u.UniformId).ToList(),
                    termin.EinsatzPlan.EinsatzplanNotenMappings.Select(n => n.NotenId).ToList());

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

                if (request.OrchestermitgliedIds is not null)
                {
                    var orchesterMitglieder = await orchesterMitgliedRepository.QueryByIdsAsync(
                        request.OrchestermitgliedIds.Select(OrchesterMitgliedsId.Create).ToArray(), cancellationToken);

                    var terminRückmeldungOrchesterMitglieder = orchesterMitglieder.Select(o =>
                        TerminRückmeldungOrchestermitglied.Create(o.Id, new List<int?> { o.DefaultInstrument },
                            new List<int?> { o.DefaultNotenStimme })).ToArray();

                    termin.UpdateTerminRückmeldungOrchestermitglied(terminRückmeldungOrchesterMitglieder);
                }

                var newTerminData = new TerminData(
                    termin.TerminStatus,
                    termin.EinsatzPlan.StartZeit,
                    termin.EinsatzPlan.EndZeit,
                    termin.EinsatzPlan.Treffpunkt,
                    termin.Dokumente.Select(d => d.Name).ToList(),
                    termin.EinsatzPlan.EinsatzplanUniformMappings.Select(u => u.UniformId).ToList(),
                    termin.EinsatzPlan.EinsatzplanNotenMappings.Select(n => n.NotenId).ToList());

                //TODO Vllt auslagern in ein Domain Event aber trotzdem noch in einer Transaktion
                var author = await _currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
                var notificationId =
                    await PublishChangeNotification(termin, oldTerminData, newTerminData, author.GetName(),
                        request.ShouldEmailBeSend);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                if (notificationId is not null)
                {
                    await _notificationBackgroundService.EnqueueNotificationAsync(notificationId);
                }

                return termin;
            }

            private async Task<NotificationId?> PublishChangeNotification(
                OrchesterApp.Domain.TerminAggregate.Termin termin,
                TerminData oldTerminData,
                TerminData newTerminData,
                string author,
                bool shouldEmailBeSend)
            {
                if (termin.IsInPast() || oldTerminData == newTerminData)
                {
                    return null;
                }

                var terminDataChangedNotification =
                    ChangeTerminDataNotification.New(termin.Id, oldTerminData, newTerminData, author, termin.Name, termin.EinsatzPlan.StartZeit);

                var mitgliederForNotification = termin.TerminRückmeldungOrchesterMitglieder
                    .Where(r => r.Zugesagt is (int)RückmeldungsartEnum.Zugesagt
                        or (int)RückmeldungsartEnum.NichtZurückgemeldet)
                    .Select(r => r.OrchesterMitgliedsId).ToList();

                List<NotificationSink> notificationSinks = [NotificationSink.Portal];

                if (shouldEmailBeSend)
                {
                    notificationSinks.Add(NotificationSink.Email);
                }

                await _notifyService.PublishNotificationAsync(terminDataChangedNotification, mitgliederForNotification,
                    notificationSinks);

                return terminDataChangedNotification.Id;
            }
        }
    }
}