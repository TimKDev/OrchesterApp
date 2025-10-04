using TvJahnOrchesterApp.Application.Features.PortalPushMessage.Models;

namespace TvJahnOrchesterApp.Application.Features.PortalPushMessage.Messages;

public class MitgliedOnlineStatusPortalPushMessage : Models.PortalPushMessage
{
    private readonly Guid _orchesterMitgliedsId;

    public override PortalPushMessageType Type => PortalPushMessageType.MitgliedOnlineStatus;

    public override object Data => new
    {
        Id = _orchesterMitgliedsId
    };

    public MitgliedOnlineStatusPortalPushMessage(PortalPushNotificationUserGroup userGroup, Guid orchesterMitgliedsId) :
        base(userGroup)
    {
        _orchesterMitgliedsId = orchesterMitgliedsId;
    }
}