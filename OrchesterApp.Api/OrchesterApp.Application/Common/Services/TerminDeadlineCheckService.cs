using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchesterApp.Domain.Common.Enums;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.NotificationAggregate.Notifications;
using OrchesterApp.Domain.NotificationAggregate.ValueObjects;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using OrchesterApp.Domain.TerminAggregate;
using OrchesterApp.Domain.UserNotificationAggregate.Enums;
using TvJahnOrchesterApp.Application.Common.Interfaces;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;

namespace TvJahnOrchesterApp.Application.Common.Services;

public class TerminDeadlineCheckService : ITerminDeadlineCheckService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INotificationBackgroundService _notificationBackgroundService;

    public TerminDeadlineCheckService(IServiceProvider serviceProvider, INotificationBackgroundService notificationBackgroundService)
    {
        _serviceProvider = serviceProvider;
        _notificationBackgroundService = notificationBackgroundService;
    }

    public async Task CheckTerminDeadlinesAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var terminRepository = scope.ServiceProvider.GetRequiredService<ITerminRepository>();
        var notifyService = scope.ServiceProvider.GetRequiredService<INotifyService>();
        var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var now = DateTime.UtcNow;

        var futureTermine = await terminRepository.GetFutureTerminsAsync(cancellationToken);
        
        var terminsWithMissingResponses = futureTermine.Where(termin =>
            termin.TerminRückmeldungOrchesterMitglieder.Any(r =>
                r.Zugesagt == (int)RückmeldungsartEnum.NichtZurückgemeldet)).ToList();
        
        var notifications = await notificationRepository.QueryByTerminAndCategoryAsync(terminsWithMissingResponses.Select(x => x.Id).ToList(), [NotificationCategory.TerminMissingResponse, NotificationCategory.TerminReminderBeforeDeadline], cancellationToken);
        
        var notificationIdsToBeSend = new List<NotificationId>();

        foreach (var termin in terminsWithMissingResponses)
        {
            var notificationIds = await HandleTerminAsync(termin, now, notifications, notifyService, cancellationToken);
            notificationIdsToBeSend.AddRange(notificationIds);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var notificationId in notificationIdsToBeSend)
        {
            await _notificationBackgroundService.EnqueueNotificationAsync(notificationId);
        }
    }

    private async Task<List<NotificationId>> HandleTerminAsync( Termin termin, DateTime now,
        Notification[] notifications, INotifyService notifyService,CancellationToken cancellationToken)
    {
        var deadlineDateTime = termin.GetDeadlineDateTime();
        var warningDateTime = termin.GetWarningDateTime();
        var result = new List<NotificationId>();

        var membersWithoutResponse = termin.TerminRückmeldungOrchesterMitglieder
            .Where(r => r.Zugesagt == (int)RückmeldungsartEnum.NichtZurückgemeldet)
            .ToList();

        if (!membersWithoutResponse.Any())
        {
            return result;
        }

        var memberWithoutResponseIds = membersWithoutResponse.Select(m => m.OrchesterMitgliedsId).ToList();

        if (warningDateTime.HasValue && now >= warningDateTime.Value && 
            now < (deadlineDateTime ?? DateTime.MaxValue) && 
            !notifications.Any(n => n.TerminId! == termin.Id && n.Category == NotificationCategory.TerminReminderBeforeDeadline))
        {
            
            var reminderNotification = TerminReminderNotification.New(
                termin.Id,
                termin.Name,
                termin.EinsatzPlan.StartZeit,
                deadlineDateTime!.Value
            );
            
            await notifyService.PublishNotificationAsync(
                reminderNotification,
                memberWithoutResponseIds,
                [NotificationSink.Portal, NotificationSink.Email],
                cancellationToken
            );
            
            result.Add(reminderNotification.Id);
        }

        if (deadlineDateTime.HasValue && now >= deadlineDateTime.Value &&
            !notifications.Any(n => n.TerminId! == termin.Id && n.Category == NotificationCategory.TerminMissingResponse))
        {
            var missingResponseNotification = TerminMissingResponseNotification.New(
                termin.Id,
                termin.Name,
                termin.EinsatzPlan.StartZeit
            );

            await notifyService.PublishNotificationAsync(
                missingResponseNotification,
                memberWithoutResponseIds,
                [NotificationSink.Portal, NotificationSink.Email],
                cancellationToken
            );
            
            result.Add(missingResponseNotification.Id);
        }
        
        return result;
    }
}

