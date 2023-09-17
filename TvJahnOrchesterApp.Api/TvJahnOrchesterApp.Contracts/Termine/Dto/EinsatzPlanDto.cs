using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;
using TvJahnOrchesterApp.Domain.TerminAggregate.Enums;

namespace TvJahnOrchesterApp.Contracts.Termine.Dto
{
    public record EinsatzPlanDto(DateTime StartZeit, DateTime EndZeit, AdresseDto Treffpunkt, List<ZeitBlockDto> ZeitBlocks, List<Noten> Noten, List<Uniform> Uniform, string? WeitereInformationen);

    public record ZeitBlockDto(Guid ZeitBlockId, DateTime Startzeit, DateTime Endzeit, string Beschreibung, AdresseDto? Adresse);
}

