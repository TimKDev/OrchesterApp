using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.OrchesterMitglied;

namespace TvJahnOrchesterApp.Application.Features
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsFeatures(this IEndpointRouteBuilder app)
        {
            app.RegisterEndpointsOrchesterMitgliedFeature();
        }
    }
}
