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

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult All()
        {
            return View(db.Admins);
        }
        #region Login/Logout
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string bannerID)
        {
            long bannerIDNum;
            Member member;
            //make sure the given bannerID is a number.
            if (Int64.TryParse(bannerID, out bannerIDNum))
            {
                member = db.Members.SingleOrDefault(f => f.Member_BannerID == bannerIDNum);
                //make sure the bannerID is actually associated with a member.
                if (member != null)
                {
                    HttpCookie authCookie = new HttpCookie("BYXInfo");
                    authCookie.Value = member.Member_ID.ToString();
                    authCookie.Expires = DateTime.Now.AddHours(1);
                    Response.Cookies.Add(authCookie);
                }
                else
                {
                    ViewBag.BannerID = bannerIDNum;
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            HttpCookie cookie = Request.Cookies["BYXInfo"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region ManageAdmins
        /// <summary>
        /// Partial View used in both Admins Edit and Create pages.
        /// </summary>
        /// <param name="id">ID will be passed in for edit pages.</param>
        /// <returns>Partial View of a form.</returns>
        public ActionResult ManageAdmin(int? id)
        {
            return PartialView();
        }

        /// <summary>
        /// GET for Editing an Admin. 
        /// </summary>
        /// <param name="id">Admin_ID of the admin to edit.</param>
        /// <returns>Html page for editing an admin.</returns>
        [HttpGet]
        public ActionResult EditAdmin(int id)
        {
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return RedirectToAction("PageNotFound", "Error");
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

        /// <summary>
        /// POST for editing an admin.
        /// </summary>
        /// <param name="admin">The EF Object of the admin to be edited.</param>
        /// <returns>Either an error view (unlikely) or redirect to Admin/All page.</returns>
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

        /// <summary>
        /// GET for the CreateAdmin page. Loads in a partial from ManageAdmin with no ID
        /// </summary>
        /// <returns>Html page with a blank form to create a new admin with.</returns>
        [HttpGet]
        public ActionResult CreateAdmin()
        {
            Admin admin = new Admin();
            var memberSelectListSource = db.Members.Where(f => f.MemberType_ID == 1).OrderBy(f => f.Member_FirstName).Select(f => new
            {
                Member_ID = f.Member_ID,
                Member_Name = f.Member_FirstName + " " + f.Member_LastName
            });
            ViewBag.Members = new SelectList(memberSelectListSource, "Member_ID", "Member_Name");
            ViewBag.Roles = new SelectList(db.AdminRoles, "Role_ID", "Role_Name");
            return View(admin);
        }

        /// <summary>
        /// POST for creating an admin.
        /// </summary>
        /// <param name="admin">EF Object of the new admin to be added to the database.</param>
        /// <returns>Redirect to home page (or possibly spit back to this page if error)</returns>
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
        #endregion
    }
}