using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BYX.Models;
namespace BYX.Utilities
{
    public class Content
    {
        public static string GetHTML(string id) {
            using (AuburnBYXDBEntities db = new AuburnBYXDBEntities())
            {
                ConMan conman = db.ConMen.FirstOrDefault(f => f.ID == id);
                return conman != null ? conman.HTML : "";
            }
        }
    }
}