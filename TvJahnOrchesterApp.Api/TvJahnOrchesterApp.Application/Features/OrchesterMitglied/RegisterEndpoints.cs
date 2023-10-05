using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints;
using TvJahnOrchesterApp.Application.Features.TerminDashboard;
using TvJahnOrchesterApp.Application.Features.TerminDashboard.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsTerminDashboardFeature(this IEndpointRouteBuilder app) 
        {
            app.MapCreateOrchesterMitgliedEndpoint();
            app.MapDeleteOrchesterMitgliedEndpoint();
            app.MapOrchesterMitgliedGetAllEndpoint();
        }
    }
}
