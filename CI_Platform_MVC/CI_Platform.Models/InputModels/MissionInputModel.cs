using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.InputModels
{
    public class MissionInputModel
    {
        public List<string> Cities { get; set; } = new List<string>();
        public List<string> Countries { get; set; } = new List<string>();
        public List<string> Themes { get; set; } = new List<string>();
        public List<string> Skills { get; set; } = new List<string>();
        public string SearchText { get; set; } = string.Empty;
        public int PageSize { get; set; } = 3;
        public int Page { get; set; } = 1;
        public string SortOrder { get; set; } = string.Empty;
    }
}
