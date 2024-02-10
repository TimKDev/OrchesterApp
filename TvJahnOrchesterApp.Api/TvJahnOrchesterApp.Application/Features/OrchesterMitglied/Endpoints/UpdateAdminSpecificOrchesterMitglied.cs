using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Infrastructure.Persistence.Repositories;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints
{
    public static class UpdateAdminSpecificOrchesterMitglied
    {
        public static void MapOrchesterMitgliedUpdateAdminSpecificEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/orchester-mitglied/admin/specific", UpdateAdminSpecificOrchesterMitglieder)
            .RequireAuthorization(auth =>
            {
                auth.RequireRole(new string[] { RoleNames.Admin, RoleNames.Vorstand });
            });
        }

        private static async Task<IResult> UpdateAdminSpecificOrchesterMitglieder(UpdateAdminSpecificOrchesterMitgliederQuery updateSpecificQuery, ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(updateSpecificQuery);
            return Results.Ok("Orchestermitglied wurde erfolgreich geupdated.");
        }

        private record UpdateAdminSpecificOrchesterMitgliederQuery(Guid Id, string Vorname, string Nachname, string Straße, string Hausnummer, string Postleitzahl, string Stadt, string Zusatz, DateTime? Geburtstag, string Telefonnummer, string Handynummer, int? DefaultInstrument, int? DefaultNotenStimme, int? OrchesterMitgliedsStatus, DateTime? MemberSince, int[] Positions, string? Image) : IRequest<Unit>;

        private class UpdateAdminSpecificOrchesterMitgliederQueryHandler : IRequestHandler<UpdateAdminSpecificOrchesterMitgliederQuery, Unit>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IUnitOfWork unitOfWork;

            public UpdateAdminSpecificOrchesterMitgliederQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(UpdateAdminSpecificOrchesterMitgliederQuery request, CancellationToken cancellationToken)
            {
                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(OrchesterMitgliedsId.Create(request.Id), cancellationToken);
                var adresse = Adresse.Create(request.Straße, request.Hausnummer, request.Postleitzahl, request.Stadt, request.Zusatz);

                orchesterMitglied.AdminUpdates(request.Vorname, request.Nachname, adresse, request.Geburtstag, request.Telefonnummer, request.Handynummer, request.DefaultInstrument, request.DefaultNotenStimme, request.OrchesterMitgliedsStatus, request.MemberSince, request.Positions, request.Image);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
