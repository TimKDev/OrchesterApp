using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.UserNotificationAggregate.ValueObjects;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace TvJahnOrchesterApp.Application.Features.Notification.Endpoints;

public static class AcknowledgeNotification
{
    public static void MapAcknowledgeNotificationEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/notifications/acknowledge/", AcknowledgeNotificationForUser)
            .RequireAuthorization();
    }

    private static async Task<IResult> AcknowledgeNotificationForUser(AcknowledgeNotificationCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.Ok();
    }

    private record AcknowledgeNotificationCommand(List<Guid> UserNotificationIds) : IRequest;

    private class AcknowledgeNotificationCommandHandler : IRequestHandler<AcknowledgeNotificationCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AcknowledgeNotificationCommandHandler(ICurrentUserService currentUserService,
            IUserNotificationRepository userNotificationRepository, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _userNotificationRepository = userNotificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AcknowledgeNotificationCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = await _currentUserService.GetCurrentUserIdAsync(cancellationToken);
            var userNotifications =
                await _userNotificationRepository.GetByIds(
                    request.UserNotificationIds.Select(UserNotificationId.Create).ToList(), cancellationToken);

            foreach (var userNotification in userNotifications.Where(u => u.UserId == currentUserId))
            {
                userNotification.MarkAsRead();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}