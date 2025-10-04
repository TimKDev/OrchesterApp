using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;

namespace OrchesterApp.Infrastructure.PortalPushMessages;

[Authorize]
public class PortalPushMessageHub : Hub
{
    private readonly ICurrentUserService _currentUserService;
    public const string AdminAndVorstandGroupName = "AdminAndVorstand";

    public PortalPushMessageHub(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        if (await _currentUserService.IsUserVorstand(CancellationToken.None))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, AdminAndVorstandGroupName);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (await _currentUserService.IsUserVorstand(CancellationToken.None))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, AdminAndVorstandGroupName);
        }
    }
}