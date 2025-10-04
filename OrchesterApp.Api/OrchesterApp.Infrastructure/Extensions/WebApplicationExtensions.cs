using Microsoft.AspNetCore.Builder;
using OrchesterApp.Infrastructure.PortalPushMessages;

namespace OrchesterApp.Infrastructure
{
    public static class WebApplicationExtensions
    {
        public static void MapPortalPushMessages(this WebApplication app)
        {
            app.MapHub<PortalPushMessageHub>("/hubs/portal-push-message");
        }
    }
}