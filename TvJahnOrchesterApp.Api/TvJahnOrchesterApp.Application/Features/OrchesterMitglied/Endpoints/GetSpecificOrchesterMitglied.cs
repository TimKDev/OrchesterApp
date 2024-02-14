using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Services;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Infrastructure.Persistence.Repositories;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints
{
    public static class GetSpecificOrchesterMitglied
    {
        public static void MapOrchesterMitgliedGetSpecificEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/orchester-mitglied/specific/{mitgliedsId}", GetSpecificOrchesterMitglieder)
            .RequireAuthorization();
        }

        private static async Task<IResult> GetSpecificOrchesterMitglieder(Guid mitgliedsId, ISender sender, CancellationToken cancellationToken)
        {
            var specificOrchesterMitglied = await sender.Send(new GetSpecificOrchesterMitgliederQuery(mitgliedsId));
            return Results.Ok(specificOrchesterMitglied);
        }

        private record GetSpecificOrchesterMitgliederResponse(Guid Id, string Vorname, string Nachname, Adresse Adresse, DateTime? Geburtstag, string? Telefonnummer, string? Handynummer, int? DefaultInstrument, int? DefaultNotenStimme, DateTime? MemberSince, int? MemberSinceInYears, int? OrchesterMitgliedsStatus, int[] Positions, string? Image);

        private record GetSpecificOrchesterMitgliederQuery(Guid Id) : IRequest<GetSpecificOrchesterMitgliederResponse>;

        private class GetSpecificOrchesterMitgliederQueryHandler : IRequestHandler<GetSpecificOrchesterMitgliederQuery, GetSpecificOrchesterMitgliederResponse>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

            public GetSpecificOrchesterMitgliederQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<GetSpecificOrchesterMitgliederResponse> Handle(GetSpecificOrchesterMitgliederQuery request, CancellationToken cancellationToken)
            {
                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(OrchesterMitgliedsId.Create(request.Id), cancellationToken);
                var imageAsBase64 = TransformImageService.ConvertByteArrayToBase64(orchesterMitglied.Image);

                return new GetSpecificOrchesterMitgliederResponse(orchesterMitglied.Id.Value, orchesterMitglied.Vorname, orchesterMitglied.Nachname, orchesterMitglied.Adresse, orchesterMitglied.Geburtstag, orchesterMitglied.Telefonnummer, orchesterMitglied.Handynummer, orchesterMitglied.DefaultInstrument, orchesterMitglied.DefaultNotenStimme, orchesterMitglied.MemberSince, orchesterMitglied.MemberSinceInYears, orchesterMitglied.OrchesterMitgliedsStatus, orchesterMitglied.PositionMappings.Select(m => m.PositionId).ToArray(), imageAsBase64);
            }
        }
    }
}
