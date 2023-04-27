using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.InputModels
{
    public class StoryInputModel
    {
        public List<string> Cities { get; set; } = new List<string>();
        public List<string> Countries { get; set; } = new List<string>();
        public List<string> Themes { get; set; } = new List<string>();
        public List<string> Skills { get; set; } = new List<string>();
        public string SearchText { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public int PageSize { get; set; } = 3;
        public int Page { get; set; } = 1;
        public long UserId { get; set; }
        public long StoryId { get; set; }
        public long MissionId { get; set; }
        public string Title { get; set; } = String.Empty;
        public string PublishedDate { get; set; } = String.Empty;
        public string StoryDescription { get; set; } = string.Empty;
        public List<IFormFile>? Media { get; set; }
        public string Type { get; set; } = string.Empty;
        public List<string> VideoUrls { get; set; } = new List<string>();

    }
}
