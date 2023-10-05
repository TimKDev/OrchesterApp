using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.Einsatzplan.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.Einsatzplan
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsEinsatzplanFeature(this IEndpointRouteBuilder app)
        {
            app.MapDeleteZeitblockEndpoint();
            app.MapUpdateEinsatzplanEndpoint();
            app.MapUpdateZeitblockEndpoint();
        }
    }
}
