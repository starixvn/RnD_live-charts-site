// --------------------------------------------------------------------------
// <copyright file="P3PAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Web.Filters
{
    using System.Web.Mvc;

    /// <summary>
    /// Privacy Preferences Project (P3P) serve a compact policy (a "p3p" HTTP header) for all requests
    /// P3P provides a standard way for Web sites to communicate about their practices around the collection,
    /// use, and distribution of personal information. It's a machine-readable privacy policy that can be
    /// automatically fetched and viewed by users, and it can be tailored to fit your company's specific policies.
    /// </summary>
    public class P3PAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// On Action Executing add a compact policy "p3p" HTTP header.
        /// </summary>
        /// <param name="filterContext">Current filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

            base.OnActionExecuting(filterContext);
        }
    }
}