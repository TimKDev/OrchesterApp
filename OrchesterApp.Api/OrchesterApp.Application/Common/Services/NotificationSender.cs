using MediatR;
using Microsoft.Extensions.Logging;
using OrchesterApp.Domain.NotificationAggregate;
using TvJahnOrchesterApp.Application.Common.ExtensionMethods;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Common.Services;

public static class NotificationSender
{
    public record Command(IList<NotificationId> NotificationIds) : IRequest;

    private class NotificationSenderCommandHandler : IRequestHandler<Command>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;

        public NotificationSenderCommandHandler(INotificationRepository notificationRepository,
            IUserNotificationRepository userNotificationRepository)
        {
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var (userNotifications, notifications) =
                await _userNotificationRepository.GetByNotificationIds(request.NotificationIds, cancellationToken)
                    .Parallelize(_notificationRepository.GetByIds(request.NotificationIds, cancellationToken));

            foreach (var notificationId in request.NotificationIds)
            {
            }
        }

        private Task HandleNotificationAsync()
        {
        }
    }
}

public class NotificationEmailSender : INotificationEmailSender
{
    public Task SendEmailsForNotificationAsync(Notification notification, IList<UserNotification> userNotifications,
        CancellationToken cancellationToken)
    {
    }
}

public interface INotificationEmailSender
{
    Task SendEmailsForNotificationAsync(Notification notification, IList<UserNotification> userNotifications,
        CancellationToken cancellationToken);
}

public interface INotificationCategoryEmailSender
{
    NotificationCategory NotificationCategory { get; }

    Task<List<Message>> CreateMessageAsync(Notification notification, IList<UserNotification> userNotification,
        CancellationToken cancellationToken);
}

public class ChangeTerminDataEmailSender : INotificationCategoryEmailSender
{
    private readonly ILogger<ChangeTerminDataEmailSender> _logger;

    public ChangeTerminDataEmailSender(ILogger<ChangeTerminDataEmailSender> logger)
    {
        _logger = logger;
    }

    public NotificationCategory NotificationCategory => NotificationCategory.ChangeTerminData;

    public async Task<List<Message>> CreateMessageAsync(Notification notification,
        IList<UserNotification> userNotification,
        CancellationToken cancellationToken)
    {
        if (notification is not ChangeTerminDataNotification changeTerminDataNotification)
        {
            _logger.LogError("Invalid notification type.");
            return [];
        }
    }
}