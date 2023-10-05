using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
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
            app.MapPost("api/OrchesterMitglied", PostOrchesterMitglied)
                .RequireAuthorization();
        }

        public static async Task<IResult> PostOrchesterMitglied([FromBody] CreateOrchesterMitgliedCommand deleteOrchesterMitgliedCommand, CancellationToken cancellationToken, ISender sender)
        {
            var result = await sender.Send(deleteOrchesterMitgliedCommand);
            //TTODO: Map
            return Results.Ok(result);
        }

        public record CreateOrchesterMitgliedCommand(string Vorname, string Nachname, AdresseDto Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, int DefaultInstrument, int DefaultNotenStimme, int[] Position, string RegisterKey) : IRequest<Domain.OrchesterMitgliedAggregate.OrchesterMitglied>;

        public class CreateOrchesterMitgliedCommandValidation : AbstractValidator<CreateOrchesterMitgliedCommand>
        {
            public CreateOrchesterMitgliedCommandValidation()
            {
                RuleFor(x => x.Vorname).NotEmpty();
                RuleFor(x => x.Nachname).NotEmpty();
            }
        }

        public class CreateOrchesterMitgliedCommandHandler : IRequestHandler<CreateOrchesterMitgliedCommand, Domain.OrchesterMitgliedAggregate.OrchesterMitglied>
        {
            private readonly IOrchesterMitgliedRepository _orchesterMitgliedRepository;

            public CreateOrchesterMitgliedCommandHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                _orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied> Handle(CreateOrchesterMitgliedCommand request, CancellationToken cancellationToken)
            {
                var adresse = Adresse.Create(request.Adresse.Straße, request.Adresse.Hausnummer, request.Adresse.Postleitzahl, request.Adresse.Stadt);

                if (await _orchesterMitgliedRepository.GetByNameAsync(request.Vorname, request.Nachname, cancellationToken) is not null)
                {
                    throw new DuplicatedOrchesterMitgliedsNameException($"Name: {request.Vorname} {request.Nachname} existiert bereits.");
                }

                var orchesterMitglied = Domain.OrchesterMitgliedAggregate.OrchesterMitglied.Create(request.Vorname, request.Nachname, adresse, request.Geburtstag, request.Telefonnummer, request.Handynummer, request.DefaultInstrument, request.DefaultNotenStimme, request.RegisterKey, (int)MitgliedsStatusEnum.aktiv);

                await _orchesterMitgliedRepository.CreateAsync(orchesterMitglied, cancellationToken);
                return orchesterMitglied;

            }
        }
    }
}
