﻿using Microsoft.AspNetCore.Http;
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
        [MinLength(3, ErrorMessage = "FirstName is too short")]
        [MaxLength(16, ErrorMessage = "FirstName is too big")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MinLength(3, ErrorMessage = "LastName is too short")]
        [MaxLength(16, ErrorMessage = "LastName is too big")]
        public string LastName { get; set; } = string.Empty;

        public IFormFile? Avatar { get; set; }

        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;

        [Required]
        public long? CityId { get; set; }
        [Required]
        public long? CountryId { get; set; }
        [Required]
        [MinLength(20, ErrorMessage = "Bio Is Too Short")]
        public string ProfileText { get; set; } = string.Empty;


        [Required]
        public string Email { get; set; } = string.Empty;
        //public User User { get; set; } = new User();

        public List<Country> Countries { get; set; } = new List<Country>();

        public List<City> Cities { get; set; } = new List<City>();

        public int Status { get; set; }
    }
}

