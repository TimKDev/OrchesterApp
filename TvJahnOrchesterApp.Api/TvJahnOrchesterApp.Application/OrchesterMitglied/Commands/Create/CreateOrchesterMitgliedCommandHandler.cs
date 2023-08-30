using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.OrchesterMitglied.Common.Errors;
using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create
{
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
            var instrument = Instrument.Create(request.DefaultInstrument.Name, MapEnumByName<ArtInstrument>(request.DefaultInstrument.ArtInstrument));
            var notenstimme = MapEnumByName<Notenstimme>(request.DefaultNotenStimme);

            if(await _orchesterMitgliedRepository.GetByNameAsync(request.Vorname, request.Nachname, cancellationToken) is not null)
            {
                throw new DuplicatedOrchesterMitgliedsNameException($"Name: {request.Vorname} {request.Nachname} existiert bereits.");
            }

            var orchesterMitglied = Domain.OrchesterMitgliedAggregate.OrchesterMitglied.Create(request.Vorname, request.Nachname, adresse, request.Geburtstag, request.Telefonnummer, request.Handynummer, instrument, notenstimme);

            await _orchesterMitgliedRepository.CreateAsync(orchesterMitglied, cancellationToken);
            return orchesterMitglied;

        }

        private TTargetEnum MapEnumByName<TTargetEnum>(Enum sourceEnum)
        {
            return (TTargetEnum)Enum.Parse(typeof(TTargetEnum), sourceEnum!.ToString()!);
        }
    }
}
