using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "FirstName Is Too Short")]
        public string FirstName { get; set; } = String.Empty;
        [Required]
        [MinLength(3, ErrorMessage = "LastName Is Too Short")]
        public string LastName { get; set; }   = String.Empty;

        public string Avatar { get; set; } = String.Empty;

        public string WhyIVolunteer { get; set; } = String.Empty;

        public string EmployeeId { get; set; } = String.Empty;

        public string Department { get; set; } = String.Empty;
        public string Availablity { get; set; } = String.Empty;
        [Required]
        public long? CityId { get; set; }
        [Required]
        public long? CountryId { get; set; }
        [Required]
        [MinLength(20, ErrorMessage = "Bio Is Too Short")]
        public string ProfileText { get; set; } = String.Empty;

        public string LinkedInUrl { get; set; } = String.Empty;

        public string Title { get; set; } = String.Empty;

        public User? User { get; set; } 

        public string Selected_Skills { get; set; } = String.Empty;
        public List<Country> Countries { get; set; } = new List<Country>();

        public List<City> Cities { get; set; } = new List<City>();

        public List<Skill> Skills { get; set; } = new List<Skill>();
    }
}
