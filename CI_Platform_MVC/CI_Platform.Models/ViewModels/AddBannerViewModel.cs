using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AddBannerViewModel
    {
        [Required]
        public IFormFile BannerImage { get; set; }

        [Required]
        public int? SortOrder { get; set; }

        [Required]
        public string BannerText { get; set; } = string.Empty;

        public int BannerId { get; set; }

        public string? BannerImagePath { get; set; } = string.Empty;

    }
}
