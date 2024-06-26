﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class SelectMissionViewModel
    {
        public string? Title { get; set; }
        public long Mission_id { get; set; }
        public string? Mission_type { get; set; } = string.Empty;

        public string? Goal_object { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public long? Goal_Achieved { get; set; }
    }
}
