// --------------------------------------------------------------------------
// <copyright file="SessionManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Web
{
    using System.Web;

    /// <summary>
    /// Handle current session.
    /// </summary>
    public class SessionManager
    {
        private const string ApolloCurrentSessionKey = "Fanex.LiveCharts.Web.CurrentSession";

        public static CurrentSession CurrentSession
        {
            get
            {
                return (CurrentSession)HttpContext.Current.Session[ApolloCurrentSessionKey];
            }

            set
            {
                HttpContext.Current.Session[ApolloCurrentSessionKey] = value;
            }
        }

        /// <summary>
        /// Check current session is log-in by admin or valid token or not.
        /// </summary>
        /// <returns>Valid or not.</returns>
        public static bool IsValid()
        {
            if (CurrentSession != null 
                && (CurrentSession.AdminId != 0 
                    || CurrentSession.MemberId != 0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clear current session.
        /// </summary>
        public static void ClearAll()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Cookies.Clear();
            }
        }
    }
}