namespace TvJahnOrchesterApp.Contracts.Common.Dto
{
    public record AdresseDto(
        string Straße,
        string Hausnummer,
        string Postleitzahl,
        string Stadt
    );
}