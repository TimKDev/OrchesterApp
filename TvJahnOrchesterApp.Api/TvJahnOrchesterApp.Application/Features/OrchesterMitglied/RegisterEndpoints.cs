using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.TerminDashboard;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsTerminDashboardFeature(this IEndpointRouteBuilder app) 
        {
            app.MapGetNextTerminEndpoint();
            app.MapNotRepliedTerminEndpoint();
        }
    }
}
