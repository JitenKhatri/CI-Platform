
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AddUserViewModel
    {

        [Required]
        [RegularExpression(@"^[^\s]{3,20}$", ErrorMessage = "First name must have between 3 and 20 characters and no spaces.")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^[^\s]{3,20}$", ErrorMessage = "Last name must have between 3 and 20 characters and no spaces.")]
        public string LastName { get; set; } = string.Empty;

        public IFormFile? Avatar { get; set; }

        public string EmployeeId { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        [Required]
        public long? CityId { get; set; }
        [Required]
        public long? CountryId { get; set; }
        [Required]
        [MinLength(20, ErrorMessage = "Bio has to have minimum 20 characters")]
        [MaxLength(70, ErrorMessage = "LastName can have maximum 70 characters")]
        public string ProfileText { get; set; } = string.Empty;


        [Required]
        [RegularExpression(@"^((([!#$%&'*+\-/=?^_`{|}~\w])|([!#$%&'*+\-/=?^_`{|}~\w][!#$%&'*+\-/=?^_`{|}~\.\w]{0,}[!#$%&'*+\-/=?^_`{|}~\w]))[@]\w+([-.]\w+)*\.\w+([-.]\w+)*)$", ErrorMessage = "Please enter a valid Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,15}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string Password { get; set; } = string.Empty;

        public string? Manager { get; set; } = string.Empty;

        public List<Country> Countries { get; set; } = new List<Country>();

        public List<City> Cities { get; set; } = new List<City>();

        public int UserId { get; set; }

        public string? AvatarPath { get; set; }

        [Required]
        public string Status { get; set; }


    }
}


