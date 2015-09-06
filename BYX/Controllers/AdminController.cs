using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BYX.Models;

namespace BYX.Controllers
{
    public class AdminController : Controller
    {
        private AuburnBYXDBEntities db = new AuburnBYXDBEntities();
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult All()
        {
            return View(db.Admins);
        }

        public ActionResult ManageAdmin(int? id)
        {
            return PartialView();
        }

        public ActionResult EditAdmin(int id)
        {
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            var memberSelectListSource = db.Members.Where(f => f.MemberType_ID == 1).OrderBy(f => f.Member_FirstName).Select(f => new {
                Member_ID = f.Member_ID,
                Member_Name = f.Member_FirstName + " " + f.Member_LastName
            });
            ViewBag.Members = new SelectList(memberSelectListSource, "Member_ID", "Member_Name", admin.Member_ID);
            ViewBag.Roles = new SelectList(db.AdminRoles, "Role_ID", "Role_Name", admin.Role_ID);
            return View(admin);
        }

        [HttpPost]
        public ActionResult EditAdmin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("All", "Admin");
            }
            var memberSelectListSource = db.Members.Where(f => f.MemberType_ID == 1).OrderBy(f => f.Member_FirstName).Select(f => new
            {
                Member_ID = f.Member_ID,
                Member_Name = f.Member_FirstName + " " + f.Member_LastName
            });
            ViewBag.Members = new SelectList(memberSelectListSource, "Member_ID", "Member_Name", admin.Member_ID);
            ViewBag.Roles = new SelectList(db.AdminRoles, "Role_ID", "Role_Name", admin.Role_ID);
            return View(admin);
        }

        public ActionResult CreateAdmin()
        {
            Admin admin = new Admin();
            var memberSelectListSource = db.Members.Where(f => f.MemberType_ID == 1).OrderBy(f => f.Member_FirstName).Select(f => new {
                Member_ID = f.Member_ID,
                Member_Name = f.Member_FirstName + " " + f.Member_LastName
            });
            ViewBag.Members = new SelectList(memberSelectListSource, "Member_ID", "Member_Name");
            ViewBag.Roles = new SelectList(db.AdminRoles, "Role_ID", "Role_Name");
            return View(admin);
        }

        [HttpPost]
        public ActionResult CreateAdmin(Admin admin)
        {

            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("All", "Admin");
            }

            var memberSelectListSource = db.Members.Where(f => f.MemberType_ID == 1).OrderBy(f => f.Member_FirstName).Select(f => new
            {
                Member_ID = f.Member_ID,
                Member_Name = f.Member_FirstName + " " + f.Member_LastName
            });
            ViewBag.Members = new SelectList(memberSelectListSource, "Member_ID", "Member_Name");
            ViewBag.Roles = new SelectList(db.AdminRoles, "Role_ID", "Role_Name");
            return View(admin);
        }
	}
}