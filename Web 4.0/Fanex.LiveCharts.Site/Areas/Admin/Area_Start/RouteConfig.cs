// --------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.13">Created</change>
// </history>

namespace Fanex.LiveCharts.Site.Areas.Admin.Area_Start
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    /// <summary>
    /// Routing config file.
    /// </summary>
    [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "EX1003:MethodMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1000:KeywordsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
    public class RouteConfig
    {
        public static void RegisterRoutes(string areaName, AreaRegistrationContext context)
        {
            context.MapRoute(
                areaName + "_Default",
                "Admin/{controller}/{action}/{id}",
                new
                {
                    controller = "Match",
                    action = "GetEnableCharts",
                    id = UrlParameter.Optional
                },
                new[] { typeof (Fanex.LiveCharts.Controllers.Admin.MatchController).Namespace });
        }
    }
}