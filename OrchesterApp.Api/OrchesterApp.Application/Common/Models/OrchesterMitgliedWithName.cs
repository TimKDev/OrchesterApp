using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Models
{
    public record OrchesterMitgliedWithName(OrchesterMitgliedsId Id, string Vorname, string Nachname);
}