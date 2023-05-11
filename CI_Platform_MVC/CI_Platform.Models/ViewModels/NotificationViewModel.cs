

namespace CI_Platform.Models.ViewModels
{
    public class NotificationViewModel
    {
        public List<NotificationSetting> NotificationSettings = new();
        public List<UserNotificationSetting> UserNotificationSettings = new();
        public List<Notification> Notifications = new();
        public int Uncheckednotificationcount;
    }
}
