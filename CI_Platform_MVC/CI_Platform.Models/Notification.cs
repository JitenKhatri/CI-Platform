using System;
using System.Collections.Generic;

namespace CI_Platform.Models;

public partial class Notification
{
    public long NotificationId { get; set; }

    public long? UserId { get; set; }

    public long? MissionId { get; set; }

    public long? StoryId { get; set; }

    public string? Message { get; set; }

    public string? Status { get; set; }

    public string? UserAvatar { get; set; }

    public string? MissionStatus { get; set; }

    public string? StoryStatus { get; set; }

    public long? NotificationSettingId { get; set; }

    public virtual Mission? Mission { get; set; }

    public virtual NotificationSetting? NotificationSetting { get; set; }

    public virtual Story? Story { get; set; }

    public virtual User? User { get; set; }
}
