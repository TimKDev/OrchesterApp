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
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;
using TvJahnOrchesterApp.Application.Features.Dropdown.Services;
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

        private record SpecificOrchesterMitgliederContract(Guid Id, string Vorname, string Nachname, Adresse Adresse, DateTime? Geburtstag, string? Telefonnummer, string? Handynummer, int? DefaultInstrument, int? DefaultNotenStimme, DateTime? MemberSince, int? MemberSinceInYears, int? OrchesterMitgliedsStatus, int[] Positions, string? Image);

        private record GetSpecificOrchesterMitgliederResponse(SpecificOrchesterMitgliederContract OrchesterMitglied, DropdownItem[] NotenstimmeDropdownItems, DropdownItem[] InstrumentDropdownItems, DropdownItem[] MitgliedsStatusDropdownItems, DropdownItem[] PositionDropdownItems);

        private record GetSpecificOrchesterMitgliederQuery(Guid Id) : IRequest<GetSpecificOrchesterMitgliederResponse>;

        private class GetSpecificOrchesterMitgliederQueryHandler : IRequestHandler<GetSpecificOrchesterMitgliederQuery, GetSpecificOrchesterMitgliederResponse>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IDropdownService dropdownService;

            public GetSpecificOrchesterMitgliederQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository, IDropdownService dropdownService)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.dropdownService = dropdownService;
            }

            public async Task<GetSpecificOrchesterMitgliederResponse> Handle(GetSpecificOrchesterMitgliederQuery request, CancellationToken cancellationToken)
            {
                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(OrchesterMitgliedsId.Create(request.Id), cancellationToken);
                var imageAsBase64 = TransformImageService.ConvertByteArrayToBase64(orchesterMitglied.Image);

                var mitgliedContract = new SpecificOrchesterMitgliederContract(orchesterMitglied.Id.Value, orchesterMitglied.Vorname, orchesterMitglied.Nachname, orchesterMitglied.Adresse, orchesterMitglied.Geburtstag, orchesterMitglied.Telefonnummer, orchesterMitglied.Handynummer, orchesterMitglied.DefaultInstrument, orchesterMitglied.DefaultNotenStimme, orchesterMitglied.MemberSince, orchesterMitglied.MemberSinceInYears, orchesterMitglied.OrchesterMitgliedsStatus, orchesterMitglied.PositionMappings.Select(m => m.PositionId).ToArray(), imageAsBase64);

                var dropdownNotenstimme = await dropdownService.GetAllDropdownValuesAsync(Dropdown.Enums.DropdownNames.Notenstimme, cancellationToken);
                var dropdownInstrument = await dropdownService.GetAllDropdownValuesAsync(Dropdown.Enums.DropdownNames.Instrument, cancellationToken);
                var dropdownMitgliedStatus = await dropdownService.GetAllDropdownValuesAsync(Dropdown.Enums.DropdownNames.MitgliedsStatus, cancellationToken);
                var dropdownPosition = await dropdownService.GetAllDropdownValuesAsync(Dropdown.Enums.DropdownNames.Position, cancellationToken);

                return new GetSpecificOrchesterMitgliederResponse(mitgliedContract, dropdownNotenstimme, dropdownInstrument, dropdownMitgliedStatus, dropdownPosition);
            }
        }
    }
}
