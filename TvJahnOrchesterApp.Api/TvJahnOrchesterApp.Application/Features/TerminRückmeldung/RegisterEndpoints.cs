using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.Authorization.Endpoints;
using TvJahnOrchesterApp.Application.Features.TerminRückmeldung.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.TerminRückmeldung
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsTerminRückmeldungFeature(this IEndpointRouteBuilder app)
        {
            app.MapGetRückmeldungTerminEndpoint();
            app.MapUpdateInstrumentAndNotesRückmeldungEndpoint();
            app.MapUpdateRückmeldungEndpoint();
            app.MapUpdateRückmeldungForOtherUserEndpoint();
            app.MapUpdateAnwesenheitsListeEndpoint();
            app.MapDeleteOwnUserEndpoint();
        }
    }
}
