using TvJahnOrchesterApp.Contracts.Common.Dto;

namespace TvJahnOrchesterApp.Contracts.Termine.Einsatzplan
{
    public record UpdateCreateZeitblockResponse(DateTime StartZeit, DateTime EndZeit, AdresseDto? Adresse, string Beschreibung);
}
