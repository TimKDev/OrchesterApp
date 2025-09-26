using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsOrchestermitgliedsFeature(this IEndpointRouteBuilder app)
        {
            app.MapCreateOrchesterMitgliedEndpoint();
            app.MapDeleteOrchesterMitgliedEndpoint();
            app.MapOrchesterMitgliedGetAllEndpoint();
            app.MapOrchesterMitgliedGetSpecificEndpoint();
            app.MapOrchesterMitgliedUpdateAdminSpecificEndpoint();
            app.MapOrchesterMitgliedUpdateSpecificEndpoint();
        }
    }
}