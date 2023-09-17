using TvJahnOrchesterApp.Contracts.OrchestraMembers;

namespace TvJahnOrchesterApp.Contracts.Termine.Einsatzplan
{
    public record UpdateCreateZeitblockResponse(DateTime StartZeit, DateTime EndZeit, AdresseDto? Adresse, string Beschreibung);
}
