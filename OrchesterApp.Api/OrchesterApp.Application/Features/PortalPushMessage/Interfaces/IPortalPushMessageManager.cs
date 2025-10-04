namespace TvJahnOrchesterApp.Application.Features.PortalPushMessage.Interfaces;

public interface IPortalPushMessageManager
{
    Task SendAsync(Models.PortalPushMessage message);
}