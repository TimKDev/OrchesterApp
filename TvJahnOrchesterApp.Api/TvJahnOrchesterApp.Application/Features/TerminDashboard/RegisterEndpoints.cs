using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.OrchesterMitglied;

namespace TvJahnOrchesterApp.Application.Features.TerminDashboard
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsOrchesterMitgliedFeature(this IEndpointRouteBuilder app)
        {
            app.MapOrchesterMitgliedGetAllEndpoint();
        }
    }
}
