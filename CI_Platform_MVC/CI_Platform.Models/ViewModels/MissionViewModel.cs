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

        public ThemeViewModel Theme { get; set; } = new ThemeViewModel();
        public SkillViewModel Skill { get; set; } = new SkillViewModel();
        public CityViewModel City { get; set; } = new CityViewModel();

        public List<City> Cities { get; set; } = new List<City>();
    }
}
