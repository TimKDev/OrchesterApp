namespace TvJahnOrchesterApp.Application.Features.Termin.Interfaces;

public interface ITerminDeadlineCheckService
{
    Task CheckTerminDeadlinesAsync(CancellationToken cancellationToken);
}

public record DeadlineCheckResult(int RemindersSent, int MissingResponseNotificationsSent);