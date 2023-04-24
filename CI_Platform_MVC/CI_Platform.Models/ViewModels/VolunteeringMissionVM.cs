using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class VolunteeringMissionVM
    {
        public Mission Missions { get; set; } = new Mission();
        public List<MissionMedium> MissionMedia { get; set; } = new List<MissionMedium>();
        public MissionMedium image { get; set; } = new MissionMedium();
        public Country? Country { get; set; }
        public City? Cities { get; set; }
        public MissionTheme? theme { get; set; }
        public List<Skill>? skills { get; set; }
        public List<Comment>? comments { get; set; }
        public int? Favorite_mission { get; set; }
        public string? Mission_city { get; set; }
        public string? Mission_theme { get; set; }
        public List<MissionSkill>? MissionSkill { get; set; }
        public List<User>? users { get; set; }
        public List<Mission>? MissionList { get; set; }
        public int? Rating { get; set; }
        public List<CommentViewModel>? Comment { get; set; }
        public decimal? Avg_ratings { get; set; }
        public int? Rating_count { get; set; }
        public List<Mission>? relatedMissions { get; set; }
        public bool Applied_or_not { get; set; }
        public List<User>? Recent_volunteers { get; set; }
        public List<User>? All_volunteers { get; set; }
        public int? Total_volunteers { get; set; }
        public List<MissionDocument>? documents { get; set; }
    }
}
