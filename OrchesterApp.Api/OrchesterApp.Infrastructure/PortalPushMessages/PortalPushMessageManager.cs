using Microsoft.AspNetCore.SignalR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Features.PortalPushMessage.Interfaces;
using TvJahnOrchesterApp.Application.Features.PortalPushMessage.Models;

namespace OrchesterApp.Infrastructure.PortalPushMessages;

public class PortalPushMessageManager : IPortalPushMessageManager
{
    private readonly IHubContext<PortalPushMessageHub> _hubContext;
    private const string ReceiveEndpointName = "Receive";

    public PortalPushMessageManager(IHubContext<PortalPushMessageHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendAsync(PortalPushMessage message)
    {
        var messageToSend = new PortalPushMessageData(message.Type.ToString(), message.Data);
        switch (message.UserGroup)
        {
            case PortalPushNotificationUserGroup.All:
                await SendToAllAsync(messageToSend);
                break;
            case PortalPushNotificationUserGroup.AdminAndVorstand:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task SendToAllAsync(PortalPushMessageData message)
    {
        await _hubContext.Clients.All.SendAsync(ReceiveEndpointName, message);
    }

    private async Task SendToAdminAndVorstandAsync(PortalPushMessageData message)
    {
        await _hubContext.Clients.Group(PortalPushMessageHub.AdminAndVorstandGroupName)
            .SendAsync(ReceiveEndpointName, message);
    }

    private record PortalPushMessageData(string Type, object Data);
}