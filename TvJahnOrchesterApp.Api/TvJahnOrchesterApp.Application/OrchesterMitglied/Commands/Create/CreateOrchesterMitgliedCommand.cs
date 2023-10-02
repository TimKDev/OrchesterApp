using MediatR;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;

namespace TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create
{
    public record CreateOrchesterMitgliedCommand(string Vorname, string Nachname, AdresseDto Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, int DefaultInstrument, int DefaultNotenStimme, int[] Position, string RegisterKey) : IRequest<Domain.OrchesterMitgliedAggregate.OrchesterMitglied>;
}
