using System.Text;
using System.Text.Json;
using OrchesterApp.Domain.Common.Enums;
using OrchesterApp.Domain.NotificationAggregate.Enums;
using OrchesterApp.Domain.NotificationAggregate.Models;
using OrchesterApp.Domain.NotificationAggregate.ValueObjects;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace OrchesterApp.Domain.NotificationAggregate.Notifications;

public sealed class ChangeTerminDataNotification : Notification
{
    public int? OldTerminStatus { get; }
    public int? NewTerminStatus { get; }
    public DateTime? OldStartZeit { get; }
    public DateTime? NewStartZeit { get; }
    public DateTime? OldEndZeit { get; }
    public DateTime? NewEndZeit { get; }
    public bool HasTreffpunktChanged { get; }
    public bool HasDokumentChanged { get; }
    public bool HasUniformChanged { get; }
    public bool HasNotenChanged { get; }
    public string Author { get; }
    public string TerminName { get; }
    public DateTime TerminStartZeit { get; }

    private ChangeTerminDataNotification(NotificationId id, NotificationType type, NotificationCategory category,
        NotificationUrgency urgency, TerminId? terminId, DateTime createdAt, string? data,
        int? oldTerminStatus, int? newTerminStatus,
        DateTime? oldStartZeit, DateTime? newStartZeit, DateTime? oldEndZeit, DateTime? newEndZeit,
        bool hasTreffpunktChanged, bool hasDokumentChanged, bool hasUniformChanged, bool hasNotenChanged,
        string author, string terminName, DateTime terminStartZeit) : base(id, type,
        category, urgency, terminId, createdAt, data)
    {
        OldTerminStatus = oldTerminStatus;
        NewTerminStatus = newTerminStatus;
        OldStartZeit = oldStartZeit;
        NewStartZeit = newStartZeit;
        OldEndZeit = oldEndZeit;
        NewEndZeit = newEndZeit;
        HasTreffpunktChanged = hasTreffpunktChanged;
        HasDokumentChanged = hasDokumentChanged;
        HasUniformChanged = hasUniformChanged;
        HasNotenChanged = hasNotenChanged;
        Author = author;
        TerminName = terminName;
        TerminStartZeit = terminStartZeit;
    }

    public static ChangeTerminDataNotification Create(Notification notification)
    {
        var dataDto = notification.Data is null
            ? new ChangeTerminDataNotificationDto()
            : JsonSerializer.Deserialize<ChangeTerminDataNotificationDto>(notification.Data)
              ?? new ChangeTerminDataNotificationDto();

        return new ChangeTerminDataNotification(notification.Id, notification.Type, notification.Category,
            notification.Urgency, notification.TerminId, notification.CreatedAt, notification.Data,
            dataDto.OldTerminStatus, dataDto.NewTerminStatus, dataDto.OldStartZeit, dataDto.NewStartZeit,
            dataDto.OldEndZeit, dataDto.NewEndZeit, dataDto.HasTreffpunktChanged, dataDto.HasDokumentChanged,
            dataDto.HasUniformChanged, dataDto.HasNotenChanged, dataDto.Author, dataDto.TerminName,
            dataDto.TerminStartZeit);
    }

    public static ChangeTerminDataNotification New(TerminId terminId, TerminData oldTerminData,
        TerminData newTerminData, string author, string terminName, DateTime terminStartZeit)
    {
        var doesStartTimeChange = oldTerminData.StartZeit != newTerminData.StartZeit;
        var doesEndTimeChange = oldTerminData.EndZeit != newTerminData.EndZeit;
        var doesTerminStatusChange = oldTerminData.TerminStatus != newTerminData.TerminStatus;
        var hasTreffpunktChanged = oldTerminData.Treffpunkt != newTerminData.Treffpunkt;
        var hasDokumentChanged = !AreListsEqual(oldTerminData.Dokumente, newTerminData.Dokumente);
        var hasUniformChanged = !AreListsEqual(oldTerminData.Uniform, newTerminData.Uniform);
        var hasNotenChanged = !AreListsEqual(oldTerminData.Noten, newTerminData.Noten);

        var oldStatusValue = doesTerminStatusChange ? oldTerminData.TerminStatus : null;
        var newStatusValue = doesTerminStatusChange ? newTerminData.TerminStatus : null;
        DateTime? oldStartTimeValue = doesStartTimeChange ? oldTerminData.StartZeit : null;
        DateTime? newStartTimeValue = doesStartTimeChange ? newTerminData.StartZeit : null;
        DateTime? oldEndValue = doesEndTimeChange ? oldTerminData.EndZeit : null;
        DateTime? newEndValue = doesEndTimeChange ? newTerminData.EndZeit : null;

        var data = JsonSerializer.Serialize(new ChangeTerminDataNotificationDto()
        {
            OldTerminStatus = oldStatusValue,
            NewTerminStatus = newStatusValue,
            OldStartZeit = oldStartTimeValue,
            NewStartZeit = newStartTimeValue,
            OldEndZeit = oldEndValue,
            NewEndZeit = newEndValue,
            HasTreffpunktChanged = hasTreffpunktChanged,
            HasDokumentChanged = hasDokumentChanged,
            HasUniformChanged = hasUniformChanged,
            HasNotenChanged = hasNotenChanged,
            Author = author,
            TerminName = terminName,
            TerminStartZeit = terminStartZeit
        });

        return new ChangeTerminDataNotification(NotificationId.CreateUnique(), NotificationType.Information,
            NotificationCategory.ChangeTerminData,
            NotificationUrgency.Medium, terminId, DateTime.UtcNow, data,
            oldStatusValue,
            newStatusValue,
            oldStartTimeValue,
            newStartTimeValue,
            oldEndValue, newEndValue, hasTreffpunktChanged, hasDokumentChanged, hasUniformChanged,
            hasNotenChanged, author, terminName, terminStartZeit);
    }

    private static bool AreListsEqual<T>(IReadOnlyList<T>? list1, IReadOnlyList<T>? list2)
    {
        if (list1 is null && list2 is null) return true;
        if (list1 is null || list2 is null) return false;
        if (list1.Count != list2.Count) return false;

        return list1.OrderBy(x => x).SequenceEqual(list2.OrderBy(x => x));
    }

    public PortalNotificationContent GetPortalNotificationContent()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"{TerminName} am {TerminStartZeit:dd.MM.yyyy}: ");

        var changes = new List<string>();

        if (OldTerminStatus.HasValue && NewTerminStatus.HasValue)
        {
            changes.Add($"Status → {((TerminStatusEnum)NewTerminStatus.Value).ToString()}");
        }

        if (OldStartZeit.HasValue && NewStartZeit.HasValue)
        {
            changes.Add($"Start → {NewStartZeit.Value:dd.MM.yyyy HH:mm}");
        }

        if (OldEndZeit.HasValue && NewEndZeit.HasValue)
        {
            changes.Add($"Ende → {NewEndZeit.Value:dd.MM.yyyy HH:mm}");
        }

        if (HasTreffpunktChanged)
        {
            changes.Add("Treffpunkt wurde verändert");
        }

        if (HasDokumentChanged)
        {
            changes.Add("Dokument wurde verändert");
        }

        if (HasUniformChanged)
        {
            changes.Add("Uniform wurde verändert");
        }

        if (HasNotenChanged)
        {
            changes.Add("Noten wurden verändert");
        }

        stringBuilder.Append(string.Join(", ", changes));

        return new PortalNotificationContent("Terminänderung", stringBuilder.ToString());
    }

    [Serializable]
    private record ChangeTerminDataNotificationDto
    {
        public int? OldTerminStatus { get; init; }
        public int? NewTerminStatus { get; init; }
        public DateTime? OldStartZeit { get; init; }
        public DateTime? NewStartZeit { get; init; }
        public DateTime? OldEndZeit { get; init; }
        public DateTime? NewEndZeit { get; init; }
        public bool HasTreffpunktChanged { get; init; }
        public bool HasDokumentChanged { get; init; }
        public bool HasUniformChanged { get; init; }
        public bool HasNotenChanged { get; init; }
        public string Author { get; init; }
        public string TerminName { get; init; }
        public DateTime TerminStartZeit { get; init; }
    }
}