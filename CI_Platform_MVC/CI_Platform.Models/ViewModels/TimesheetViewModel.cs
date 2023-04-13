using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class TimesheetViewModel
    {
        public List<SelectMissionViewModel> Missions { get; set; } = new List<SelectMissionViewModel>();
        public List<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
        public long Mission_id { get; set; }
        public string? Volunteered_date { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Actions { get; set; }
        public string Message { get; set; } = String.Empty;
    }
}
