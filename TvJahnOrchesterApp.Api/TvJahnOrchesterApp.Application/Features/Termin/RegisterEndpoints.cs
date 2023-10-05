using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.Termin.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.Termin
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsTerminFeature(this IEndpointRouteBuilder app)
        {
            app.MapTerminCreateEndpoint();
            app.MapDeleteTerminEndpoint();
            app.MapGetAllTerminsEndpoint();
            app.MapGetSpecificTerminEndpoint();
            app.MapUpdateTerminEndpoint();
        }
    }
}
