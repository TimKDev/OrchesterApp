using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.Dashboard.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.Dashboard
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsOrchesterMitgliedFeature(this IEndpointRouteBuilder app)
        {
            app.MapGetDashboardEndpoint();
        }
    }
}
