using MediatR;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.Enums;

namespace TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create
{
    public record CreateOrchesterMitgliedCommand(string Vorname, string Nachname, AdresseDto Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, InstrumentDto DefaultInstrument, NotenstimmeEnum DefaultNotenStimme, Position Position, string RegisterKey) : IRequest<Domain.OrchesterMitgliedAggregate.OrchesterMitglied>;
}
