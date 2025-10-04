using TvJahnOrchesterApp.Application.Features.PortalPushMessage.Models;

namespace TvJahnOrchesterApp.Application.Features.PortalPushMessage.Messages;

public class TerminPortalPushMessage : Models.PortalPushMessage
{
    public override PortalPushMessageType Type => PortalPushMessageType.Termins;
    public override object Data => new();

    public TerminPortalPushMessage(PortalPushNotificationUserGroup userGroup) : base(userGroup)
    {
    }
}