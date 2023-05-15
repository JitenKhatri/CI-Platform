using System;
using System.Collections.Generic;

namespace CI_Platform.Models;


public partial class NotificationSetting
{
    public long NotificationSettingId { get; set; }

    public string? NotificationSettingName { get; set; }

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<UserNotificationSetting> UserNotificationSettings { get; } = new List<UserNotificationSetting>();
}
