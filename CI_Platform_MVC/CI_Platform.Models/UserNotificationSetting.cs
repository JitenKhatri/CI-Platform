using System;
using System.Collections.Generic;

namespace CI_Platform.Models;

public partial class UserNotificationSetting
{
    public long UserNotificationSettingId { get; set; }

    public long? NotificationSettingId { get; set; }

    public long? UserId { get; set; }

    public virtual NotificationSetting? NotificationSetting { get; set; }

    public virtual User? User { get; set; }
}
