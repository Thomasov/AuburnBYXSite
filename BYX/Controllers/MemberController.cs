﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BYX.Models;

namespace BYX.Controllers
{
    public class MemberController : Controller
    {
        private AuburnBYXDBEntities db = new AuburnBYXDBEntities();

        // GET: /Member/
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.MemberType);
            return View(members.ToList());
        }

        // GET: /Member/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            DateTime today = DateTime.Now;
            List<BYXEvent> Events = db.BYXEvents.Where(f => !f.Deleted && f.Event_StartDateTime < today).ToList();
            List<int> attendedEvents = member.AttendanceRecords.Select(f => f.Event_ID).ToList();
            List<AttendanceOutput> outputs = new List<AttendanceOutput>();
            foreach (BYXEvent byxEvent in Events)
            {
                outputs.Add(new AttendanceOutput(byxEvent.Event_Name, attendedEvents.Contains(byxEvent.Event_ID)));
            }
            ViewBag.Attendance = outputs;
            ViewBag.TotalAbsences = outputs.Count(f => !f.Attended);
            return View(member);
        }

        // GET: /Member/Create
        public ActionResult Create()
        {
            ViewBag.MemberType_ID = new SelectList(db.MemberTypes, "MemberType_ID", "MemberType_Name");
            return View();
        }

        // POST: /Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Member member)
        {
            if (ModelState.IsValid)
            {
                member.Deleted = false;
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberType_ID = new SelectList(db.MemberTypes, "MemberType_ID", "MemberType_Name", member.MemberType_ID);
            return View(member);
        }

        // GET: /Member/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            //ViewBag.MemberType_ID = new SelectList(db.MemberTypes, "MemberType_ID", "MemberType_Name", member.MemberType_ID);
            ViewBag.MemberTypes = db.MemberTypes;
            return View(member);
        }

        // POST: /Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Member_ID,Member_FirstName,Member_LastName,Member_EmailAddress,Member_BannerID,MemberType_ID,Deleted,Member_IgnitedNum")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberType_ID = new SelectList(db.MemberTypes, "MemberType_ID", "MemberType_Name", member.MemberType_ID);
            return View(member);
        }

        // GET: /Member/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: /Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class AttendanceOutput
    {
        public string Event_Name { get; set; }

        public bool Attended { get; set; }

        public AttendanceOutput(string eventName, bool attended)
        {
            this.Event_Name = eventName;
            this.Attended = attended;
        }
    }
}
