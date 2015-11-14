using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BYX.Models;
using BYX.Utilities;

namespace BYX.Controllers
{
    public class NonMemberController : Controller
    {
        private AuburnBYXDBEntities db = new AuburnBYXDBEntities();

        [BYXAuthorize]
        public ActionResult Potentials()
        {
            var potentials = db.Potentials.Where(f => !f.Archived).ToList();
            return View(potentials);
        }

        [BYXAuthorize("Admin")]
        public ActionResult Guardians()
        {
            var guardians = db.Guardians.Where(f => !f.Archived).ToList();
            return View(guardians);
        }
	}
}