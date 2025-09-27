using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Models
{
    public record OrchesterMitgliedAdminInfo(
        OrchesterMitgliedsId Id,
        string Vorname,
        string Nachname,
        string? ConnectedUserId,
        DateTime? UserLastLogin);
}