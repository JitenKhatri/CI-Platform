﻿using CI_Platform.Models;

namespace CI_Platform.Areas.Admin.ViewModels
{
    public class CrudViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Mission> Missions { get; set; } = new List<Mission>();

        public List<MissionTheme> MissionThemes { get; set; } = new List<MissionTheme>();

        public List<Skill> Skills { get; set; } = new List<Skill>();

        public List<Story> Stories { get; set; } = new List<Story>();
    }
}
