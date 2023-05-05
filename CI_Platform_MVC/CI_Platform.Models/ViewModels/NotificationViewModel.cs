using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class NotificationViewModel
    {
       public  List<NotificationSetting> NotificationSettings = new List<NotificationSetting>();
       public  List<UserNotificationSetting> UserNotificationSettings = new List<UserNotificationSetting>();
    }
}
