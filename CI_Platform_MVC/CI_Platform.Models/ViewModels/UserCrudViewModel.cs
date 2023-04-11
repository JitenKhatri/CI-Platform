using CI_Platform.Models;

namespace CI_Platform.Areas.Admin.ViewModels
{
    public class UserCrudViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
    }
}
