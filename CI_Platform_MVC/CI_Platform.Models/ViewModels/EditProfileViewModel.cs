using Microsoft.AspNetCore.Http;
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
        [MinLength(3, ErrorMessage = "FirstName is too short")]
        [MaxLength(16, ErrorMessage = "FirstName is too big")]
        public string FirstName { get; set; } = String.Empty;
        [Required]
        [MinLength(3, ErrorMessage = "LastName is too short")]
        [MaxLength(16, ErrorMessage = "LastName is too big")]
        public string LastName { get; set; }   = String.Empty;

        public IFormFile? Avatar { get; set; }

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

        [RegularExpression(@"^(https:\/\/)?(www\.)?linkedin\.com\/in\/[a-zA-Z0-9\-]+\/?$", ErrorMessage = "Please enter a valid LinkedIn URL.")]
        public string LinkedInUrl { get; set; } = String.Empty;

        public string? Manager { get; set; } = String.Empty;

        [Required]
        [MinLength(3, ErrorMessage = "Title is too short")]
        [MaxLength(16, ErrorMessage = "Title is too big")]
        public string Title { get; set; } = String.Empty;

        public string Profile { get; set; } = String.Empty;
        //public User User { get; set; } = new User();

        public string Selected_Skills { get; set; } = String.Empty;
        public string Selected_skill_names { get; set; } = String.Empty;
        public List<Country> Countries { get; set; } = new List<Country>();

        public List<City> Cities { get; set; } = new List<City>();

        public List<Skill> Skills { get; set; } = new List<Skill>();

        public List<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
    }
}
