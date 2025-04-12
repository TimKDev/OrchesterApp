using TvJahnOrchesterApp.Contracts.Common.Dto;

namespace TvJahnOrchesterApp.Contracts.Termine.Einsatzplan
{
    public record UpdateCreateZeitblockRequest(Guid? ZeitblockId, DateTime StartZeit, DateTime EndZeit, AdresseDto? Adresse, string Beschreibung);
}
