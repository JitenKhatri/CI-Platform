using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class StoryViewModel
    {
        public Story? Stories { get; set; }

        public StoryMedium image { get; set; }

        public List<City> Cities { get; set; }
        public List<Country> Countries { get; set; }

        public string? Story_city { get; set; }
      
        public string? Story_theme { get; set; }

        public List<StoryMedium> images { get; set; }

    }
}
