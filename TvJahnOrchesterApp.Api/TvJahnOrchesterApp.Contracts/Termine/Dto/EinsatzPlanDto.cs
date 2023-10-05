using TvJahnOrchesterApp.Contracts.OrchestraMembers;

namespace TvJahnOrchesterApp.Contracts.Termine.Dto
{
    public record EinsatzPlanDto(DateTime StartZeit, DateTime EndZeit, AdresseDto Treffpunkt, List<ZeitBlockDto> ZeitBlocks, List<int> Noten, List<int> Uniform, string? WeitereInformationen);

    public record ZeitBlockDto(Guid ZeitBlockId, DateTime Startzeit, DateTime Endzeit, string Beschreibung, AdresseDto? Adresse);
}

