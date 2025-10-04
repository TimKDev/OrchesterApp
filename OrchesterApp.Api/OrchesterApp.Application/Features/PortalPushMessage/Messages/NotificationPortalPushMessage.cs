using TvJahnOrchesterApp.Application.Features.PortalPushMessage.Models;

namespace TvJahnOrchesterApp.Application.Features.PortalPushMessage.Messages;

public class NotificationPortalPushMessage : Models.PortalPushMessage
{
    public override PortalPushMessageType Type => PortalPushMessageType.Notifications;
    public override object Data => new();

    public NotificationPortalPushMessage(PortalPushNotificationUserGroup userGroup) : base(userGroup)
    {
    }
}