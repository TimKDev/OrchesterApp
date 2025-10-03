using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.Notifications;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Application.Common.Interfaces;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;

namespace TvJahnOrchesterApp.Application.Features.Notification.Endpoints
{
    public static class SendCustomNotification
    {
        public static void MapSendCustomNotificationEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/notification/send-custom", SendCustomNotificationAsync)
                .RequireAuthorization(r => r.RequireRole(RoleNames.Admin, RoleNames.Vorstand));
        }

        private static async Task<IResult> SendCustomNotificationAsync(SendCustomNotificationCommand command, ISender sender,
            CancellationToken cancellationToken)
        {
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        }

        private record SendCustomNotificationCommand(
            string Title,
            string Message,
            bool ShouldEmailBeSend,
            Guid[] OrchestermitgliedIds) : IRequest<Unit>;

        private class SendCustomNotificationCommandHandler : IRequestHandler<SendCustomNotificationCommand, Unit>
        {
            private readonly INotifyService _notifyService;
            private readonly INotificationBackgroundService _notificationBackgroundService;
            private readonly IUnitOfWork _unitOfWork;

            public SendCustomNotificationCommandHandler(
                INotifyService notifyService,
                INotificationBackgroundService notificationBackgroundService,
                IUnitOfWork unitOfWork)
            {
                _notifyService = notifyService;
                _notificationBackgroundService = notificationBackgroundService;
                _unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(SendCustomNotificationCommand request, CancellationToken cancellationToken)
            {
                var customNotification = CustomMessageNotification.New(request.Title, request.Message);

                var mitgliederForNotification = request.OrchestermitgliedIds
                    .Select(OrchesterMitgliedsId.Create)
                    .ToList();

                List<NotificationSink> notificationSinks = [NotificationSink.Portal];

                if (request.ShouldEmailBeSend)
                {
                    notificationSinks.Add(NotificationSink.Email);
                }

                await _notifyService.PublishNotificationAsync(customNotification, mitgliederForNotification,
                    notificationSinks, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _notificationBackgroundService.EnqueueNotificationAsync(customNotification.Id);

                return Unit.Value;
            }
        }
    }
}


