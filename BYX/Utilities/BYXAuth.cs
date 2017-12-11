using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BYX.Models;
using System.Web.Mvc;

namespace BYX.Utilities
{
    public class BYXAuth
    {
        /// <summary>
        /// Checks to see whether the currently logged in person is a full admin.
        /// </summary>
        /// <returns>bool</returns>
        public static bool IsAdmin()
        {
            using (AuburnBYXDBEntities db = new AuburnBYXDBEntities())
            {
                if (!IsLoggedIn())
                {
                    return false;
                }
                int memberID = GetMemberID();
                return db.Admins.Any(f => f.AdminRole.Role_Name == "Admin" && f.Member_ID == memberID);
            }
        }

        /// <summary>
        /// Checks to see whether the person accessing this method is logged in.
        /// </summary>
        /// <returns>bool</returns>
        public static bool IsLoggedIn()
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
            if (IsLoggedIn())
            {
                int memberID = Convert.ToInt32(HttpContext.Current.Request.Cookies["BYXInfo"].Value);
                return memberID;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Determines if the currently logged in user is part of any of the given roles.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static bool IsMemberOf(params string[] roles)
        {
            using (AuburnBYXDBEntities db = new AuburnBYXDBEntities())
            {
                bool isMember = false;
                int memberID = GetMemberID();
                foreach (Admin admin in db.Admins.Where(f => f.Member_ID == memberID))
                {
                    foreach (string role in roles) 
                    {
                        if (admin.AdminRole.Role_Name.ToLower() == role.ToLower())
                        {
                            isMember = true;
                        }
                    }
                }
                return isMember;
                
            }
        }
    }

    /// <summary>
    /// AuthorizeUser Attribute. Restricts access to action methods in a controller to logged in users only. Optionally, 
    /// further restrictions can be specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class BYXAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private bool loggedIn = false;

        private string[] roles = new string[0];

        /// <summary>
        /// overloaded constructor to make using one role easier.
        /// </summary>
        /// <param name="role">The one role to authorize this method by.</param>
        public BYXAuthorizeAttribute(string role): this(new string[] { role }) { }

        /// <summary>
        /// AuthorizeUser Attribute. Restricts access to action methods in a controller to logged in users only. Optionally, 
        /// further restrictions can be specified.1
        /// </summary>
        /// <param name="allowedRoles">The role(s) required for access.</param>
        public BYXAuthorizeAttribute(string[] allowedRoles = null) 
        {
            roles = allowedRoles ?? new string[0];
        }

        /// <summary>
        /// OVERRIDDEN METHOD: The logic that determines whether a user is authorized or not. 
        /// If no roles were specified, authorize based on whether the user is logged in or not.
        /// </summary>
        /// <param name="httpContext">Current HttpContext.</param>
        /// <returns>Boolean value representing whether a user is authorized to access the action method or not.</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            loggedIn = BYXAuth.IsLoggedIn();

            if (!loggedIn)
            {
                return false;
            }
            //We have roles to check for, so we're not done yet.
            if (roles.Length != 0) 
            { 
                return BYXAuth.IsMemberOf(roles); 
            } 
            else //Else, with no roles to check, just return our login check.
            { 
                return loggedIn; 
            } 
        }

        /// <summary>
        /// OVERRIDDEN METHOD: Only called when AuthorizeCore() returns false.
        /// </summary>
        /// <param name="filterContext">Current AuthorizationContext.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (loggedIn)
            {
                filterContext.Result = new RedirectResult("~/Error/Unauthorized");
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Admin/Login", false);
            }
        }
    }
}
