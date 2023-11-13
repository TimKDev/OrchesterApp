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

        private record UpdateSpecificOrchesterMitgliederQuery(Guid Id, string Straße, string Hausnummer, string Postleitzahl, string Stadt, string Zusatz, DateTime? Geburtstag, string Handynummer, string Telefonnummer) : IRequest<Unit>;

        private class UpdateSpecificOrchesterMitgliederQueryHandler : IRequestHandler<UpdateSpecificOrchesterMitgliederQuery, Unit>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IUnitOfWork unitOfWork;

            public UpdateSpecificOrchesterMitgliederQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(UpdateSpecificOrchesterMitgliederQuery request, CancellationToken cancellationToken)
            {
                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(OrchesterMitgliedsId.Create(request.Id), cancellationToken);
                var adresse = Adresse.Create(request.Straße, request.Hausnummer, request.Postleitzahl, request.Stadt, request.Zusatz);


                orchesterMitglied.UserUpdates(adresse, request.Geburtstag, request.Telefonnummer, request.Handynummer);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
