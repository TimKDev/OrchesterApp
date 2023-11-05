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
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Infrastructure.Persistence.Repositories;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints
{
    public static class UpdateAdminSpecificOrchesterMitglied
    {
        public static void MapOrchesterMitgliedUpdateAdminSpecificEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/orchester-mitglied/specific/{mitgliedsId}", UpdateAdminSpecificOrchesterMitglieder)
            .RequireAuthorization();
        }

        private static async Task<IResult> UpdateAdminSpecificOrchesterMitglieder(UpdateAdminSpecificOrchesterMitgliederQuery updateSpecificQuery, ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(updateSpecificQuery);
            return Results.Ok("Orchestermitglied wurde erfolgreich geupdated.");
        }

        private record UpdateAdminSpecificOrchesterMitgliederQuery(Guid Id, string Vorname, string Nachname, Adresse Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, int DefaultInstrument, int DefaultNotenStimme, int MitgliedsStatus, DateTime MemberSince, int[] PositionIds) : IRequest<Unit>;

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
                orchesterMitglied.AdminUpdates(request.Vorname, request.Nachname, request.Adresse, request.Geburtstag, request.Telefonnummer, request.Handynummer, request.DefaultInstrument, request.DefaultNotenStimme, request.MitgliedsStatus, request.MemberSince, request.PositionIds);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
