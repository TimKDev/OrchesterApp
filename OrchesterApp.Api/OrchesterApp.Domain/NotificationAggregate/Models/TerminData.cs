using OrchesterApp.Domain.Common.ValueObjects;

namespace OrchesterApp.Domain.NotificationAggregate.Models;

public record TerminData(
    int? TerminStatus,
    DateTime StartZeit,
    DateTime EndZeit,
    Adresse? Treffpunkt,
    IReadOnlyList<string>? Dokumente,
    IReadOnlyList<int>? Uniform,
    IReadOnlyList<int>? Noten);