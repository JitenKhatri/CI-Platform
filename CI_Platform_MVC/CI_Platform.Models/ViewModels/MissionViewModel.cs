using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class MissionViewModel
    {

        public Mission? Missions { get; set; }

        public MissionMedium image { get; set; }
        public List<Country>? Country { get; set; }
        public List<City>? Cities { get; set; }
        public List<MissionTheme>? themes { get; set; }
        public List<Skill>? skills { get; set; }
        public List<FavoriteMission>? favoriteMissions { get; set; }
        public List<MissionRating>? missionRatings { get; set; }

        public string? Mission_city { get; set; }
        public string? Mission_theme { get; set; }
        public int? Favorite_mission { get; set; }
    }
}
