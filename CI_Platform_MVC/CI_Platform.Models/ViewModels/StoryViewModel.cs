using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class StoryViewModel
    {
        public Story? Stories { get; set; } = new Story();

        public StoryMedium Image { get; set; } = new StoryMedium();

        public List<City> Cities { get; set; } = new List<City>();
        public List<Country> Countries { get; set; } = new List<Country>();

        public string Story_city { get; set; } = String.Empty;

        public string Story_theme { get; set; } = String.Empty;

        public List<StoryMedium> Images { get; set; } = new List<StoryMedium>();

        public List<User> All_volunteers { get; set; } = new List<User>();

        public long User_id { get; set; }

        public string User_avatar { get; set; } = string.Empty;

        public string User_firstname { get; set; } = string.Empty;
        public string User_lastname { get; set; } = string.Empty;
        public long MissionId { get; set; }
        public string Missiontitle { get; set; } = string.Empty;

        public List<StoryMedium> Vidmedia { get; set; } = new List<StoryMedium>();
    }
}
