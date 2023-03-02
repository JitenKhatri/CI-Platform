using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class MissionViewModel
    {
    
        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }


        public string? OrganizationName { get; set; }


        public City city { get; set; }

        public Country country { get; set; }

       
    }
}
