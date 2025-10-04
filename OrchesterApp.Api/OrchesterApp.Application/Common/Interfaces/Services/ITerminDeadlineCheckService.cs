namespace TvJahnOrchesterApp.Application.Common.Interfaces.Services;

public interface ITerminDeadlineCheckService
{
    Task CheckTerminDeadlinesAsync(CancellationToken cancellationToken);
}

public record DeadlineCheckResult(int RemindersSent, int MissingResponseNotificationsSent);

