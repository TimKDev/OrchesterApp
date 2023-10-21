using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;

namespace TvJahnOrchesterApp.Application.Features
{
    internal static class TestEndpoint
    {
        public static void MapAddTestEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/test", async (CancellationToken cancellationToken, IEmailService emailService) =>
            {
                await emailService.SendEmailAsync(new Common.Models.Message(new string[] { "carolin.kempkens@web.de" }, "Orchester App Hello", "Dies ist eine Testmail."));
            });
        }
    }
}
