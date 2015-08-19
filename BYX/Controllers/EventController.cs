using System;
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
    public class EventController : Controller
    {
        private AuburnBYXDBEntities db = new AuburnBYXDBEntities();

        // GET: /Event/
        public ActionResult Index()
        {
            var byxevents = db.BYXEvents.Where(f => !f.Deleted).Include(b => b.EventType);
            return View(byxevents.ToList());
        }

        // GET: /Event/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BYXEvent byxevent = db.BYXEvents.Find(id);
            if (byxevent == null)
            {
                return HttpNotFound();
            }
            return View(byxevent);
        }

        // GET: /Event/Create
        public ActionResult Create()
        {
            ViewBag.EventType_ID = new SelectList(db.EventTypes, "EventType_ID", "EventType_Name");
            return View();
        }

        // POST: /Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BYXEvent byxevent)
        {
            if (ModelState.IsValid)
            {
                db.BYXEvents.Add(byxevent);
                byxevent.Event_StartDateTime = byxevent.Event_StartDateTime.ToUniversalTime();
                byxevent.Deleted = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventType_ID = new SelectList(db.EventTypes, "EventType_ID", "EventType_Name", byxevent.EventType_ID);
            return View(byxevent);
        }

        // GET: /Event/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BYXEvent byxevent = db.BYXEvents.Find(id);
            if (byxevent == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventType_ID = new SelectList(db.EventTypes, "EventType_ID", "EventType_Name", byxevent.EventType_ID);
            return View(byxevent);
        }

        // POST: /Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BYXEvent byxevent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(byxevent).State = EntityState.Modified;
                byxevent.Event_StartDateTime = byxevent.Event_StartDateTime.ToUniversalTime();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventType_ID = new SelectList(db.EventTypes, "EventType_ID", "EventType_Name", byxevent.EventType_ID);
            return View(byxevent);
        }

        // GET: /Event/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BYXEvent byxevent = db.BYXEvents.Find(id);
            if (byxevent == null)
            {
                return HttpNotFound();
            }
            return View(byxevent);
        }

        // POST: /Event/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            BYXEvent byxevent = db.BYXEvents.Find(id);
            //db.BYXEvents.Remove(byxevent);
            db.Entry(byxevent).State = EntityState.Modified;
            byxevent.Deleted = true;            
            db.SaveChanges();
            return Json(new{
                success=true
            });
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
}
