using TvJahnOrchesterApp.Contracts.Common.Dto;

namespace TvJahnOrchesterApp.Contracts.Termine.Einsatzplan
{
    public record UpdateEinsatzplanResponse(DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, int[] Noten, int[] Uniform);
}
