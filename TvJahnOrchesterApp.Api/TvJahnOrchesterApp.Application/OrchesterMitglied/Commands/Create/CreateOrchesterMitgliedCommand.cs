using MediatR;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate;

namespace TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create
{
    public record CreateOrchesterMitgliedCommand(string Vorname, string Nachname, AdresseCommand Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, InstrumentCommand DefaultInstrument, NotenstimmeCommand DefaultNotenStimme, PositionCommand Position) : IRequest<Domain.OrchesterMitgliedAggregate.OrchesterMitglied>;

    public record AdresseCommand(string Straße,
        string Hausnummer,
        string Postleitzahl,
        string Stadt);

    public record InstrumentCommand(string Name, ArtInstrumentCommand ArtInstrument);

    public enum ArtInstrumentCommand
    {
        Holz, Blech, Schlagwerk, Dirigent
    }

    public enum NotenstimmeCommand
    {
        AltSaxophon1,
        AltSaxophon2,
    }

    public enum PositionCommand
    {
        Dirigent,
        Obmann,
        Kassierer,
        Notenwart,
        Zeugwart,
        Thekenteam
    }
}
