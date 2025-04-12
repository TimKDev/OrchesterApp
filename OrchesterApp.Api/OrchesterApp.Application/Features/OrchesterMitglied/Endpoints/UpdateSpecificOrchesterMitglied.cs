using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Services;
using OrchesterApp.Domain.Common.ValueObjects;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints
{
    public static class UpdateSpecificOrchesterMitglied
    {
        public static void MapOrchesterMitgliedUpdateSpecificEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/orchester-mitglied/specific", UpdateSpecificOrchesterMitglieder)
            .RequireAuthorization();
        }

        private static async Task<IResult> UpdateSpecificOrchesterMitglieder(UpdateSpecificOrchesterMitgliederQuery updateSpecificQuery, ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(updateSpecificQuery);
            return Results.Ok("Orchestermitglied wurde erfolgreich geupdated.");
        }

        private record UpdateSpecificOrchesterMitgliederQuery(Guid Id, string Straße, string Hausnummer, string Postleitzahl, string Stadt, string Zusatz, DateTime? Geburtstag, string Handynummer, string Telefonnummer, string? Image) : IRequest<Unit>;

        private class UpdateSpecificOrchesterMitgliederQueryHandler : IRequestHandler<UpdateSpecificOrchesterMitgliederQuery, Unit>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IUnitOfWork unitOfWork;
            private readonly ICurrentUserService currentUserService;

            public UpdateSpecificOrchesterMitgliederQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.unitOfWork = unitOfWork;
                this.currentUserService = currentUserService;
            }

            public async Task<Unit> Handle(UpdateSpecificOrchesterMitgliederQuery request, CancellationToken cancellationToken)
            {
                var currentOrchesterMitglied = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
                var mitgliedsId = OrchesterMitgliedsId.Create(request.Id);
                if (mitgliedsId != currentOrchesterMitglied.Id)
                {
                    throw new Exception("Not authorized");
                }

                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(mitgliedsId, cancellationToken);
                var adresse = Adresse.Create(request.Straße, request.Hausnummer, request.Postleitzahl, request.Stadt, request.Zusatz);

                var imageAsByteArray = TransformImageService.ConvertToCompressedByteArray(request.Image);

                DateTime? geburtstagUtc = request.Geburtstag is null ? null : request.Geburtstag.Value.ToUniversalTime();

                orchesterMitglied.UserUpdates(adresse, geburtstagUtc, request.Telefonnummer, request.Handynummer, imageAsByteArray);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
