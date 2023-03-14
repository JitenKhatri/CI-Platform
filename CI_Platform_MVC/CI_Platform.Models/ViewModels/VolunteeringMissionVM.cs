﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class VolunteeringMissionVM
    {
        public Mission? Missions { get; set; }

        public MissionMedium image { get; set; }
        public Country? Country { get; set; }
        public City? Cities { get; set; }
        public MissionTheme? theme { get; set; }
        public List<Skill>? skills { get; set; }
        public List<Comment>? comments { get; set; }
        public string? Mission_city { get; set; }
        public string? Mission_theme { get; set; }
        public List<MissionSkill>? MissionSkill { get; set; }
        public List<User>? users { get; set; }
        public List<Mission>? MissionList { get; set; }

        public CommentViewModel? Comment { get; set; }
    }
}
