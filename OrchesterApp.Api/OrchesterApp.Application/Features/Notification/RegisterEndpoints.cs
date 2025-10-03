using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.Notification.Endpoints;
using TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.Notification
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsNotificationFeature(this IEndpointRouteBuilder app)
        {
            app.MapGetNotificationForUserEndpoint();
            app.MapAcknowledgeNotificationEndpoint();
            app.MapSendCustomNotificationEndpoint();
        }
    }
}