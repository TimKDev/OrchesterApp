namespace TvJahnOrchesterApp.Contracts.OrchestraMembers
{
    public record AdresseDto(
        string Straße,
        string Hausnummer,
        string Postleitzahl,
        string Stadt
    );
}