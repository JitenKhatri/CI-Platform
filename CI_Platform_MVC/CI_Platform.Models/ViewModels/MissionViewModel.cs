using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class MissionViewModel
    {

        public Mission Missions { get; set; } = new Mission();

        public MissionMedium image { get; set; } = new MissionMedium();
        public List<FavoriteMission> favoriteMissions { get; set; } = new List<FavoriteMission>();
        public List<MissionRating> missionRatings { get; set; } = new List<MissionRating>();

        public string Mission_city { get; set; } = String.Empty;
        public string Mission_theme { get; set; } = String.Empty;
        public int Favorite_mission { get; set; }

        public CountryViewModel Country { get; set; } = new CountryViewModel();

        public ThemeViewModel themes { get; set; } = new ThemeViewModel();
        public SkillViewModel skills { get; set; } = new SkillViewModel();
        public CityViewModel Cities { get; set; } = new CityViewModel();
    }
}
