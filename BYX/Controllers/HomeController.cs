﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BYX.Models;

namespace BYX.Controllers
{
	public class HomeController : Controller
	{
		private AuburnBYXDBEntities db = new AuburnBYXDBEntities();

		/// <summary>
		/// Method for returning the view of the home page.
		/// </summary>
		/// <returns>ActionResult (View of home page)</returns>
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Members() 
		{
			return View(db.Members.Where(f => !f.Deleted).ToList());
		}


		[HttpGet]
		public ActionResult GetName(string input)
		{
			long cardNum = Convert.ToInt64(input.Split(new char[] { ';', '=' })[1]);
			Member member = db.Members.SingleOrDefault(f => f.Member_IgnitedNum == cardNum);
			return Json(new {
				firstName = member.Member_FirstName,
				lastName = member.Member_LastName
			}, JsonRequestBehavior.AllowGet);
		}

        public ActionResult Register()
        {
            return View(new NonMember());
        }

        public ActionResult Resources()
        {
            return View();
        }

        public ActionResult ChapterDress()
        {
            return View();
        }

        public ActionResult BlockSeating()
        {
            return View();
        }
	}
}