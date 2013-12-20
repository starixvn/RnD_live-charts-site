// --------------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.7">Created</change>
// </history>

namespace Fanex.LiveCharts.Authentication.WebApi
{
    using System.Web.Http;

    /// <summary>
    /// Web API routing configurations.
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new 
                { 
                    id = RouteParameter.Optional 
                });
        }
    }
}