using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints;
using TvJahnOrchesterApp.Application.Features.TerminDashboard.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.TerminDashboard
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsOrchesterMitgliedFeature(this IEndpointRouteBuilder app)
        {
            app.MapGetNextTerminEndpoint();
            app.MapNotRepliedTerminEndpoint();
        }
    }
}
