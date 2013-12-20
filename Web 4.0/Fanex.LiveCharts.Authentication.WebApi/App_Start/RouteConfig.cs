// --------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.7">Created</change>
// </history>

namespace Fanex.LiveCharts.Authentication.WebApi
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Routing configurations.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Register routes information.
        /// </summary>
        /// <param name="routes">Route table.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new 
                { 
                    controller = "Home", 
                    action = "Index", 
                    id = UrlParameter.Optional 
                });
        }
    }
}