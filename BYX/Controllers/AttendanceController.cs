using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BYX.Models;
using BYX.Utilities;
using System.Net.Mail;
using System.Data.Entity;

namespace BYX.Controllers
{
    public class AttendanceController : Controller
    {
        private AuburnBYXDBEntities db = new AuburnBYXDBEntities();

        //
        // GET: /Attendance/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Swipe(int? id) 
        {
            if (id == null) 
            {
                return RedirectToAction("Index", "Error");
            }
            BYXEvent byxEvent = db.BYXEvents.Find(id);
            if (byxEvent == null)
            {
                return RedirectToAction("Index", "Error");
            }
            return View(byxEvent);
        }

        [HttpPost]
        public ActionResult SwipeCreate(string input, int eventID)
        {
            if (!String.IsNullOrEmpty(input))
            {
                long cardNumber;
                BYXEvent byxEvent = db.BYXEvents.Find(eventID);
                if (byxEvent == null)
                {
                    return Json(new {
                        success = false,
                        errormessage = "This event doesn't exist."
                    });
                }
                Member member = new Member();
                bool isValidNumber = true;
                //check if the ignited number is a real number, if it is get the member
                if (long.TryParse(input, out cardNumber))
                {
                    member = db.Members.SingleOrDefault(f => f.Member_IgnitedNum == cardNumber);
                    if (member == null)
                    {
                        member = db.Members.SingleOrDefault(f => f.Member_BannerID == cardNumber);
                        if (member == null)
                        {
                            isValidNumber = false;
                        }
                    }
                }
                else
                {
                    isValidNumber = false;
                }
                if (!isValidNumber)
                {
                    return Json(new {
                        success = false,
                        errormessage = "Please make sure you're using an Auburn Ignited card."
                    });
                }
                //check if this member has already swiped into this event.
                if (db.AttendanceRecords.Any(f => f.Event_ID == eventID && f.Member_ID == member.Member_ID))
                {
                    return Json(new{
                        success = false,
                        errormessage = member.Member_FirstName + " " + member.Member_LastName + " has already swiped into this event."
                    });
                }
                AttendanceRecord swipe = new AttendanceRecord();
                swipe.SwipeTime = DateTime.UtcNow;
                swipe.Member_ID = member.Member_ID;
                swipe.Event_ID = eventID;
                swipe.isExcusedAbsence = false;
                db.AttendanceRecords.Add(swipe);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        errormessage = "Something went wrong. Sorry :("
                    });
                }
                //SmtpClient client = new SmtpClient();
                //MailAddress from = new MailAddress("teo0004@auburn.edu");
                //MailAddress to = new MailAddress(member.Member_EmailAddress);
                //MailMessage message = new MailMessage(from, to);
                //message.Subject = "Attendance for " + byxEvent.Event_Name + " on " + byxEvent.Event_StartDateTime.ToShortDateString();
                //message.Body = "<p>" + member.Member_FirstName + ",</p><br />"
                //                + "<p>You have successfully swiped in for " + byxEvent.Event_Name + "</p>"
                //                + "You have a total of " + BYXToolkit.GetTotalAbsences(member) + " total absences. For a more detailed report of your absences, please mail the secretary.";
                //message.IsBodyHtml = true;
                //client.Host = "tigermail.auburn.edu";
                //client.UseDefaultCredentials = false;
                //client.Port = 25;
                //client.Send(message);
                return Json(new{
                    success = true,
                    fullname = member.Member_FirstName + " " + member.Member_LastName
                });
            }
            else
            {
                return Json(new {
                    success = false
                });
            }
        }

        public ActionResult PickEvent()
        {
            ViewBag.Events = getTodaysEvents();
            return View();
        }

        public ActionResult EventReport(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            BYXEvent byxEvent = db.BYXEvents.Find(id);
            if (byxEvent == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            
            EventReportViewModel reportVM = new EventReportViewModel();
            reportVM.eventReports = db.sp_EventAttendanceRecord(id).OrderBy(f => f.Name).ToList();
            reportVM.Event = byxEvent;
            return View(reportVM);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult ChangeAttendance(int eventID, int memberID, string attendanceType)
        {
            Member member = db.Members.Find(memberID);
            BYXEvent byxEvent = db.BYXEvents.Find(eventID);

            if (member == null || byxEvent == null)
            {
                return Json(new {
                    success = false
                });
            }
            AttendanceRecord record = db.AttendanceRecords.SingleOrDefault(f => f.Event_ID == eventID && f.Member_ID == memberID);
            switch (attendanceType)
            {
                default:
                case "Excused":
                case "Attended":
                    if (record != null) {
                        db.Entry(record).State = EntityState.Modified;
                        record.isExcusedAbsence = (attendanceType == "Excused") ? true : false;
                        record.SwipeTime = byxEvent.Event_StartDateTime;
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            return Json(new
                            {
                                success = false,
                                errormessage = "Something went wrong. Sorry :("
                            });
                        }
                    }
                    else
                    {
                        record = new AttendanceRecord();
                        record.SwipeTime = byxEvent.Event_StartDateTime;
                        record.Member_ID = memberID;
                        record.Event_ID = eventID;
                        record.isExcusedAbsence = (attendanceType == "Excused") ? true : false;
                        db.AttendanceRecords.Add(record);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            return Json(new
                            {
                                success = false,
                                errormessage = "Something went wrong. Sorry :("
                            });
                        }
                    }
                    break;
                case "Late":
                    break;
                case "Absent":
                    break;
            }

            return Json(new { 
                success = true
            });
        }

        private List<BYXEvent> getTodaysEvents() 
        {
            List<BYXEvent> todaysEvents = new List<BYXEvent>();
            DateTime today = DateTime.UtcNow.Date;
            todaysEvents = db.BYXEvents.Where(f => DateTime.Compare(f.Event_StartDateTime, today) >= 0).ToList();
            return todaysEvents;

        }

        
	}
}