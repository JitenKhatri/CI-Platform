using CI_Platform.Models;

namespace CI_Platform.Areas.Admin.ViewModels
{
    public class CrudViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Mission> Missions { get; set; } = new List<Mission>();

        public List<MissionTheme> MissionThemes { get; set; } = new List<MissionTheme>();

        public List<Skill> Skills { get; set; } = new List<Skill>();

        public List<Story> Stories { get; set; } = new List<Story>();

        public List<MissionApplication> MissionApplications { get; set; } = new List<MissionApplication>();

        public List<CmsPage> CmsPages { get; set; } = new List<CmsPage>();

        public List<Banner> Banners { get; set; } = new List<Banner>();

        public List<Timesheet> Timesheets { get; set; } = new List<Timesheet>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
