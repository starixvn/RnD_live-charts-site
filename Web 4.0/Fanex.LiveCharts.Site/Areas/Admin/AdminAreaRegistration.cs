// --------------------------------------------------------------------------
// <copyright file="AdminAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.9">Created</change>
// </history>

namespace Fanex.LiveCharts.Site.Areas.Admin
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using Fanex.LiveCharts.Site.Areas.Admin.Area_Start;

    /// <summary>
    /// Register area for admin area.
    /// </summary>
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        /// <summary>
        /// Register admin area.
        /// </summary>
        /// <param name="context">Current context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
