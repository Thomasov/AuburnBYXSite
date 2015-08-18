using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BYX.Models
{
    public class EventReportViewModel
    {
        public BYXEvent Event { get; set; }

        public List<sp_EventAttendanceRecord_Result> eventReports { get; set; }
    }
}