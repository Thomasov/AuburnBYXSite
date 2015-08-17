using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BYX.Models;
namespace BYX.Utilities
{
    public class BYXToolkit
    {
        public static int GetTotalAbsences(Member member)
        {
            using (BYXEntities db = new BYXEntities())
            {
                int totalAbsences = 0;
                DateTime today = DateTime.Now;
                List<BYXEvent> Events = db.BYXEvents.Where(f => !f.Deleted && f.Event_StartDateTime < today).ToList();
                List<int> attendedEvents = member.AttendanceRecords.Select(f => f.Event_ID).ToList();
                foreach (BYXEvent byxEvent in Events)
                {
                    if (!attendedEvents.Contains(byxEvent.Event_ID)) {
                        totalAbsences++;
                    }
                }
                return totalAbsences;
            }
        }
    }
}