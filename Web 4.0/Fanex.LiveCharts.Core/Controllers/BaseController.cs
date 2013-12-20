// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.7">Created</change>
// </history>

namespace Fanex.LiveCharts.Controllers
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Fanex.LiveCharts.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseController : Controller
    {
        private static readonly CultureInfo DefaultCulture = CultureInfo.CreateSpecificCulture("en-US");

        /// <summary>
        /// Set current language.
        /// </summary>
        /// <param name="filterContext">Current filter context.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionManager.CurrentSession != null
                && SessionManager.CurrentSession.MemberId != 0
                && !string.IsNullOrEmpty(SessionManager.CurrentSession.Language))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(SessionManager.CurrentSession.Language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SessionManager.CurrentSession.Language);
                Thread.CurrentThread.CurrentCulture.DateTimeFormat = DefaultCulture.DateTimeFormat;
                Thread.CurrentThread.CurrentCulture.NumberFormat = DefaultCulture.NumberFormat;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}