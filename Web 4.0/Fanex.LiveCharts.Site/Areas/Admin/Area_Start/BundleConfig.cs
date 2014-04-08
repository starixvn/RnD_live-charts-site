// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.16">Created</change>
// </history>

namespace Fanex.LiveCharts.Site.Areas.Admin.Area_Start
{
    using System.Web.Optimization;

    /// <summary>
    /// Register bundle.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Register bundle for admin site.
        /// </summary>
        /// <param name="bundles">Bundle collections.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/admin/adminscripts").Include(
                ////"~/Areas/Admin/Scripts/modernizr-2.5.3.js",
                "~/Areas/Admin/Scripts/html5shiv.js",
                "~/Areas/Admin/scripts/_Layout.js",
                "~/Areas/Admin/scripts/live-charts-admin-site.js",
                "~/Areas/UserLogin/scripts/ReCheckIn.js"));

            bundles.Add(new ScriptBundle("~/areas/admin/getenablechartjs").Include(
               "~/Areas/Admin/Scripts/live-charts-enable-charts.js"));

            bundles.Add(new ScriptBundle("~/areas/admin/getmatchdetailjs").Include(
                "~/Areas/Admin/Scripts/live-charts-match-detail.js"));

            bundles.Add(new ScriptBundle("~/areas/admin/searchmatchjs").Include(
                "~/Areas/Admin/Scripts/live-charts-search-matches.js"));

            bundles.Add(new ScriptBundle("~/areas/admin/scorepredictionconfigjs").Include(
                "~/Scripts/Libs/Spinner/jquery.stepper.js",
                "~/Areas/Admin/Scripts/ScorePrediction/score-prediction-config.js",
                "~/Areas/Admin/Scripts/ScorePrediction/score-prediction-config-view.js"));

            bundles.Add(new ScriptBundle("~/areas/admin/chatmonitorjs").Include(
                "~/Scripts/Libs/Moment/moment.js",
                "~/Scripts/Libs/Fanex.Grid/Fanex.Pagination.js",
                "~/Scripts/Libs/Fanex.Grid/Fanex.Grid.js",
                "~/Areas/Admin/Scripts/Chat/chat-monitor.js"));

            bundles.Add(new ScriptBundle("~/areas/admin/chatsettingconfigjs").Include(
                "~/Areas/Admin/Scripts/Setting/chat-setting.js"));
        }
    }
}