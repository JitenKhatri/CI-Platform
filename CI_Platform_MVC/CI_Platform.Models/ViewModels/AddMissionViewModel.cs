﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AddMissionViewModel
    {

        public List<MissionMedium> MissionMedia { get; set; } = new List<MissionMedium>();

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string ShortDescription { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public long CityId { get; set; }
        [Required]
        public long CountryId { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string OrganizationDetail { get; set; } = string.Empty;

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }
        [Required]
        public string MissionType { get; set; } = string.Empty;
        public long SeatsLeft { get; set; }
        public DateTime? Deadline { get; set; }

        [Required]
        public long ThemeId { get; set; }

        [Required]
        public string Availability { get; set; } = string.Empty;

        public List<Country> Countries { get; set; } = new List<Country>();

        public List<City> Cities { get; set; } = new List<City>();
        public List<MissionTheme> Themes { get; set; } = new List<MissionTheme>();
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public string? Selected_Skills { get; set; } = string.Empty;

        public string? Goal_Motto { get; set; } = string.Empty;
        public string Selected_skill_names { get; set; } = string.Empty;
        [Required]
        public List<IFormFile>? Media { get; set; }
        [Required]
        public List<IFormFile>? MissionDocuments { get; set; }
        public List<string>? VideoUrls { get; set; } = new List<string>();
        public List<MissionSkill> MissionSkills { get; set; } = new List<MissionSkill>();
        public List<MissionDocument> ExistingDocuments { get; set; } = new List<MissionDocument>();

        public long MissionId { get; set; }

        [Required]
        [RegularExpression(@"^https?:\/\/(?:www\.|m\.)?youtube\.com\/watch\?v=([a-zA-Z0-9_-]{11})$", ErrorMessage = "Please enter a valid YouTube video URL.")]
        public string YoutubeUrl { get; set; } = string.Empty;

    }
}
