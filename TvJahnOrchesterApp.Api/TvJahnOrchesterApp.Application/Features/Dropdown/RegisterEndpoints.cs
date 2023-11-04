using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Features.Dropdown.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.Dropdown
{
    public static class RegisterEndpoints
    {
        public static void RegisterEndpointsDaashboard(this IEndpointRouteBuilder app)
        {
            app.MapGetDropdownEndpoint();
        }
    }
}
