namespace TvJahnOrchesterApp.Application.Features.PortalPushMessage.Models;

public abstract class PortalPushMessage
{
    public PortalPushNotificationUserGroup UserGroup { get; }
    public abstract PortalPushMessageType Type { get; }
    public abstract object Data { get; }

    protected PortalPushMessage(PortalPushNotificationUserGroup userGroup)
    {
        UserGroup = userGroup;
    }
}