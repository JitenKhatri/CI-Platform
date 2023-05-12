using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AddCMSViewModel
    {
        public long CMSPageId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Title has to have at least 3 characters")]
        [MaxLength(30, ErrorMessage = "Title can only have 30 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(20, ErrorMessage = "Description has to have at least 20 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; }
    }
}
