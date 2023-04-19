﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AddMissionViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Required]
        public long CityId { get; set; }
        [Required]
        public long CountryId { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string OrganizationDetail { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; } = new DateOnly();
        public DateOnly EndDate { get; set; } = new DateOnly();
        public string MissionType { get; set; } = string.Empty;
        public long SeatsLeft { get; set; }
        public DateOnly Deadline { get; set; } = new DateOnly();
        public long ThemeId { get; set; }
        public string Availability { get; set; } = string.Empty;

        public List<Country> Countries { get; set; } = new List<Country>();

        public List<City> Cities { get; set; } = new List<City>();
        public List<MissionTheme> Themes { get; set; } = new List<MissionTheme>();
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public string? Selected_Skills { get; set; } = string.Empty;

        public string? Goal_Motto { get; set; } = string.Empty;
        public string Selected_skill_names { get; set; } = string.Empty;
        public List<MissionSkill> MissionSkills { get; set; } = new List<MissionSkill>();
    }
}