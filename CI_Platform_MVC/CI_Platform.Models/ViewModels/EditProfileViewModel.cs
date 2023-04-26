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
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MinLength(3, ErrorMessage = "LastName is too short")]
        [MaxLength(16, ErrorMessage = "LastName is too big")]
        public string LastName { get; set; }   = string.Empty;

        public IFormFile? Avatar { get; set; }

        [Required]
        public string WhyIVolunteer { get; set; } = string.Empty;

        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;

        [Required]
        public string Availablity { get; set; } = string.Empty;
        [Required]
        public long? CityId { get; set; }
        [Required]
        public long? CountryId { get; set; }
        [Required]
        [MinLength(20, ErrorMessage = "Required minimum length is 20")]
        public string ProfileText { get; set; } = string.Empty;

        [RegularExpression(@"^(https:\/\/)?(www\.)?linkedin\.com\/in\/[a-zA-Z0-9\-]+\/?$", ErrorMessage = "Please enter a valid LinkedIn URL.")]
        public string LinkedInUrl { get; set; } = string.Empty;

        public string? Manager { get; set; } = string.Empty;

        [Required]
        [MinLength(3, ErrorMessage = "Title has to have at least 3 characters")]
        [MaxLength(16, ErrorMessage = "Title can have maximum 16 characters ")]
        public string Title { get; set; } = string.Empty;

        
        public string Profile { get; set; } = string.Empty;

        
        public string Email { get; set; } = string.Empty;

        public string? Selected_Skills { get; set; } = string.Empty;

        public string Selected_skill_names { get; set; } = string.Empty;
        public List<Country> Countries { get; set; } = new List<Country>();

        public List<City> Cities { get; set; } = new List<City>();

        public List<Skill> Skills { get; set; } = new List<Skill>();

        public List<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
    }
}
