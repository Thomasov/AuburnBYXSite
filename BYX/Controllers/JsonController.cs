using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BYX.Models;
using Newtonsoft.Json;

namespace BYX.Controllers
{
    public class JsonController : Controller
    {

        public string GetMembers()
        {
            using (AuburnBYXDBEntities db = new AuburnBYXDBEntities())
            {
                var members = db.Members.Where(f => !f.Deleted && (f.MemberType.MemberType_Name == "Active Brother" || f.MemberType.MemberType_Name == "Pledge")).OrderBy(f => f.Member_LastName).Select(f => new
                {
                    Member_ID = f.Member_ID,
                    Member_Name = f.Member_FirstName + " " + f.Member_LastName
                });
                return JsonConvert.SerializeObject(members);
            }
        }

        public string GetGuardianTypes()
        {
            using (AuburnBYXDBEntities db = new AuburnBYXDBEntities())
            {
                var guardianTypes = db.GuardianTypes.Select(f => new
                {
                    GuardianType_ID = f.GuardianType_ID,
                    GuardianType_Label = f.GuardianType_Label
                });
                return JsonConvert.SerializeObject(guardianTypes);
            }
        }
    }
}