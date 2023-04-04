using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.InputModels
{
    public class TimesheetInputModel
    {
        public long Mission_id { get; set; }
        public string Volunteered_date { get; set; } = String.Empty;
        public int Actions { get; set; }
        public string Message { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty;
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Timesheet_id { get; set; }
        public string Date { get; set; } = String.Empty;

    }
}
