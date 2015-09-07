using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BYX.Models;

namespace BYX.Utilities
{
    public class BYXAuth
    {
        /// <summary>
        /// Checks to see whether the currently logged in person is a full admin.
        /// </summary>
        /// <returns>bool</returns>
        public static bool IsFullAdmin() 
        {
            using (AuburnBYXDBEntities db = new AuburnBYXDBEntities())
            {
                if (!isLoggedIn())
                {
                    return false;
                }
                int memberID = GetMemberID();
                if (db.Admins.Any(f => f.AdminRole.Role_Name == "Full Admin" && f.Member_ID == memberID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Checks to see whether the person accessing this method is logged in.
        /// </summary>
        /// <returns>bool</returns>
        public static bool isLoggedIn()
        {
            if (HttpContext.Current.Request.Cookies["BYXInfo"] != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the Member_ID of the currently logged in Member.
        /// </summary>
        /// <returns>Member_ID if logged in, 0 otherwise.</returns>
        public static int GetMemberID()
        {
            if (isLoggedIn())
            {
                int memberID = Convert.ToInt32(HttpContext.Current.Request.Cookies["BYXInfo"].Value);
                return memberID;
            }
            else
            {
                return 0;
            }
        }
    }
}