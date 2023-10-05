using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.AnwesenheitsListe.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.AnwesenheitsListe
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsAnwesenheitsListeFeature(this IEndpointRouteBuilder app)
        {
            app.MapGetAllAnwesenheitsListeEndpoint();
            app.MapGetTerminAnwesenheitsListeEndpoint();
            app.MapUpdateAnwesenheitsListeEndpoint();
        }
    }
}
