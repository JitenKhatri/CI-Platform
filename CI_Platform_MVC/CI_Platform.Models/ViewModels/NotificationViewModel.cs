

namespace CI_Platform.Models.ViewModels
{
    public class NotificationViewModel
    {
        public List<NotificationSetting> NotificationSettings = new List<NotificationSetting>();
        public List<UserNotificationSetting> UserNotificationSettings = new List<UserNotificationSetting>();
        public List<Notification> Notifications = new List<Notification>();
        public int Uncheckednotificationcount;
    }
}
