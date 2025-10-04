using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Application.Features.Termin.Interfaces;

namespace TvJahnOrchesterApp.Application.Features.Termin.Endpoints;

public static class TriggerDeadlineCheck
{
    public static void MapTriggerDeadlineCheckEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/termin/check-deadlines", TriggerDeadlineCheckHandler)
            .RequireAuthorization(r => r.RequireRole(RoleNames.Admin));
    }

    private static async Task<IResult> TriggerDeadlineCheckHandler(ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(new TriggerDeadlineCheckCommand(), cancellationToken);
        return Results.Ok();
    }

    private record TriggerDeadlineCheckCommand : IRequest;

    private class TriggerDeadlineCheckCommandHandler : IRequestHandler<TriggerDeadlineCheckCommand>
    {
        private readonly ITerminDeadlineCheckService _deadlineCheckService;

        public TriggerDeadlineCheckCommandHandler(ITerminDeadlineCheckService deadlineCheckService)
        {
            _deadlineCheckService = deadlineCheckService;
        }

        public Task Handle(TriggerDeadlineCheckCommand request, CancellationToken cancellationToken)
        {
            return _deadlineCheckService.CheckTerminDeadlinesAsync(cancellationToken);
        }
    }
}