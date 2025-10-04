using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Features.Notification.Interfaces;
using TvJahnOrchesterApp.Application.Features.Notification.Models;

namespace TvJahnOrchesterApp.Application.Features.Notification.Endpoints;

public static class GetNotificationsForUser
{
    public static void MapGetNotificationForUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/notifications/user/", QueryNotificationsForUser)
            .RequireAuthorization();
    }

    private static async Task<IResult> QueryNotificationsForUser(ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetNotificationsForUserQuery(), cancellationToken);
        return Results.Ok(response);
    }

    private record GetNotificationsForUserQuery() : IRequest<NotificationsForUserResponse>;

    private record NotificationsForUserResponse(IList<NotificationDto> Notifications);

    private class
        NotificationsForUserQueryHandler : IRequestHandler<GetNotificationsForUserQuery, NotificationsForUserResponse>
    {
        private const int NumberOfNotifications = 20;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IPortalNotificationBuilder _portalNotificationBuilder;

        public NotificationsForUserQueryHandler(ICurrentUserService currentUserService,
            IUserNotificationRepository userNotificationRepository, INotificationRepository notificationRepository,
            IPortalNotificationBuilder portalNotificationBuilder)
        {
            _currentUserService = currentUserService;
            _userNotificationRepository = userNotificationRepository;
            _notificationRepository = notificationRepository;
            _portalNotificationBuilder = portalNotificationBuilder;
        }

        public async Task<NotificationsForUserResponse> Handle(GetNotificationsForUserQuery request,
            CancellationToken cancellationToken)
        {
            var result = new List<NotificationDto>();
            var currentUserId = await _currentUserService.GetCurrentUserIdAsync(cancellationToken);

            var userNotifications =
                await _userNotificationRepository.GetByUserId(currentUserId, NumberOfNotifications, cancellationToken);

            var notifications =
                await _notificationRepository.GetByIds(
                    userNotifications.Select(x => x.NotificationId).Distinct().ToList(), cancellationToken);

            foreach (var userNotification in userNotifications)
            {
                var notification = notifications.FirstOrDefault(n => n.Id == userNotification.NotificationId);

                if (notification is null)
                {
                    continue;
                }

                var portalMessage = _portalNotificationBuilder.Build(notification);

                result.Add(new NotificationDto(
                    userNotification.Id.Value, portalMessage.Title, portalMessage.Message, userNotification.IsRead,
                    notification.Type,
                    notification.Urgency,
                    notification.TerminId?.Value, notification.CreatedAt
                ));
            }

            return new NotificationsForUserResponse(result.OrderByDescending(n => n.CreatedAt).ToList());
        }
    }
}