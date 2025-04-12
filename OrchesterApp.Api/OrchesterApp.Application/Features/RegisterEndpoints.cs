using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common;
using TvJahnOrchesterApp.Application.Features.Abstimmung;
using TvJahnOrchesterApp.Application.Features.AnwesenheitsListe;
using TvJahnOrchesterApp.Application.Features.Authorization;
using TvJahnOrchesterApp.Application.Features.Dashboard;
using TvJahnOrchesterApp.Application.Features.Dropdown;
using TvJahnOrchesterApp.Application.Features.Einsatzplan;
using TvJahnOrchesterApp.Application.Features.OrchesterMitglied;
using TvJahnOrchesterApp.Application.Features.Termin;
using TvJahnOrchesterApp.Application.Features.TerminRückmeldung;

namespace TvJahnOrchesterApp.Application.Features
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsFeatures(this IEndpointRouteBuilder app)
        {
            app.RegisterEndpointsOrchesterMitgliedFeature();
            app.RegisterEndpointsTerminDashboardFeature();
            app.RegisterEndpointsAbstimmungFeature();
            app.RegisterEndpointsAuthorizationFeature();
            app.RegisterEndpointsEinsatzplanFeature();
            app.RegisterEndpointsTerminFeature();
            app.RegisterEndpointsTerminRückmeldungFeature();
            app.RegisterEndpointsAnwesenheitsListeFeature();
            app.RegisterEndpointsDropdown();
        }
    }
}
