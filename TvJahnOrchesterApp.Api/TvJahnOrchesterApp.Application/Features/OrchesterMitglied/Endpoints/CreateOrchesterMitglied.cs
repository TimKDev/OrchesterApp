using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Models.Errors;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints
{
    public static class CreateOrchesterMitglied
    {
        public static void MapCreateOrchesterMitgliedEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/orchester-mitglied", PostOrchesterMitglied)
                .RequireAuthorization(auth =>
                {
                    auth.RequireRole(new string[] { RoleNames.Admin, RoleNames.Vorstand });
                });
        }

        private static async Task<IResult> PostOrchesterMitglied([FromBody] CreateOrchesterMitgliedCommand createOrchesterMitgliedCommand, ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(createOrchesterMitgliedCommand);
            return Results.Ok("Orchestermitglied wurde erfolgreich erstellt.");
        }

        private record CreateOrchesterMitgliedCommand(string Vorname, string Nachname, string Straße, string Hausnummer, string Postleitzahl, string Stadt, string Zusatz, DateTime? Geburtstag, string Telefonnummer, string Handynummer, int? DefaultInstrument, int? DefaultNotenStimme, int[] Position, string RegisterKey, DateTime? MemberSince) : IRequest<Unit>;

        private class CreateOrchesterMitgliedCommandValidation : AbstractValidator<CreateOrchesterMitgliedCommand>
        {
            public CreateOrchesterMitgliedCommandValidation()
            {
                RuleFor(x => x.Vorname).NotEmpty();
                RuleFor(x => x.Nachname).NotEmpty();
                RuleFor(x => x.RegisterKey).MinimumLength(6);
            }
        }

        private class CreateOrchesterMitgliedCommandHandler : IRequestHandler<CreateOrchesterMitgliedCommand, Unit>
        {
            private readonly IOrchesterMitgliedRepository _orchesterMitgliedRepository;

            public CreateOrchesterMitgliedCommandHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                _orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<Unit> Handle(CreateOrchesterMitgliedCommand request, CancellationToken cancellationToken)
            {
                var adresse = Adresse.Create(request.Straße, request.Hausnummer, request.Postleitzahl, request.Stadt);

                if (await _orchesterMitgliedRepository.GetByNameAsync(request.Vorname, request.Nachname, cancellationToken) is not null)
                {
                    throw new DuplicatedOrchesterMitgliedsNameException($"Name: {request.Vorname} {request.Nachname} existiert bereits.");
                }

                var orchesterMitglied = Domain.OrchesterMitgliedAggregate.OrchesterMitglied.Create(request.Vorname, request.Nachname, adresse, request.Geburtstag, request.Telefonnummer, request.Handynummer, request.DefaultInstrument, request.DefaultNotenStimme, request.RegisterKey, (int)MitgliedsStatusEnum.aktiv);

                orchesterMitglied.UpdatePositions(request.Position);
                orchesterMitglied.SetMemberSince(request.MemberSince);

                await _orchesterMitgliedRepository.CreateAsync(orchesterMitglied, cancellationToken);
                return Unit.Value;

            }
        }
    }
}
