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
        public City? Cities { get; set; } = new City(); 
        public MissionTheme? theme { get; set; } = new MissionTheme(); 
        public List<Skill>? skills { get; set; } = new List<Skill>(); 
        public List<Comment>? comments { get; set; } = new List<Comment>(); 
        public int? Favorite_mission { get; set; } 
        public int? Rating { get; set; } 
        public List<CommentViewModel>? Comment { get; set; } = new List<CommentViewModel>();
        public decimal? Avg_ratings { get; set; } 
        public int? Rating_count { get; set; } 
        public List<Mission>? relatedMissions { get; set; } = new List<Mission>(); 
        public bool Applied_or_not { get; set; } 
        public List<User>? Recent_volunteers { get; set; } = new List<User>(); 
        public List<User>? All_volunteers { get; set; } = new List<User>(); 
        public int? Total_volunteers { get; set; } 
        public List<MissionDocument>? documents { get; set; } = new List<MissionDocument>(); 
    } 
}
