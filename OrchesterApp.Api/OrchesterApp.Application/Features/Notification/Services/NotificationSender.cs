using MediatR;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.ValueObjects;
using OrchesterApp.Domain.UserNotificationAggregate;
using OrchesterApp.Domain.UserNotificationAggregate.Enums;
using TvJahnOrchesterApp.Application.Common.ExtensionMethods;
using TvJahnOrchesterApp.Application.Common.Interfaces.Notifications;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace TvJahnOrchesterApp.Application.Common.Services;

public static class NotificationSender
{
    public record Command(IList<NotificationId> NotificationIds) : IRequest;

    private class NotificationSenderCommandHandler : IRequestHandler<Command>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly INotificationEmailSender _notificationEmailSender;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationSenderCommandHandler(INotificationRepository notificationRepository,
            IUserNotificationRepository userNotificationRepository, INotificationEmailSender notificationEmailSender,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
            _notificationEmailSender = notificationEmailSender;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var userNotifications =
                await _userNotificationRepository.GetByNotificationIds(request.NotificationIds, cancellationToken);

            var notifications = await _notificationRepository.GetByIds(request.NotificationIds, cancellationToken);

            var notificationDict =
                notifications.ToDictionary(n => n.Id,
                    n => (Notification: NotificationFactory.Create(n),
                        UserNotifications: userNotifications.Where(u => u.NotificationId == n.Id).ToList()));

            foreach (var notificationId in request.NotificationIds)
            {
                if (!notificationDict.TryGetValue(notificationId, out var selectedNotification))
                {
                    continue;
                }

                var emailUserNotifications = selectedNotification.UserNotifications
                    .Where(u => u.NotificationSink == NotificationSink.Email)
                    .ToList();

                await _notificationEmailSender.SendEmailsForNotificationAsync(selectedNotification.Notification,
                    emailUserNotifications, cancellationToken);

                await SetUserNotificationStatusToSendedAsync(emailUserNotifications, cancellationToken);
            }
        }

        private async Task SetUserNotificationStatusToSendedAsync(List<UserNotification> emailUserNotifications,
            CancellationToken cancellationToken)
        {
            foreach (var userNotification in emailUserNotifications)
            {
                userNotification.SendedSuccessfully();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}