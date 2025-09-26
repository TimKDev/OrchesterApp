using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.Dashboard.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.Dashboard
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsDashboardFeature(this IEndpointRouteBuilder app)
        {
            app.MapGetDashboardEndpoint();
        }
    }
}