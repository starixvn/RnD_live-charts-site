// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Site
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "EX1003:MethodMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/Libs/jquery-1.9.1.js")
                .Include("~/Scripts/Libs/BlockUI/jquery.blockUI.js")
                .Include("~/Scripts/LIVECHARTS.js"));

            RegisterChartBundles(bundles);
            RegisterScorePredictionBundles(bundles);
            RegisterChatBundles(bundles);
        }

        public static void RegisterChartBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/chart")
                .Include("~/Scripts/Libs/HighChart/highcharts.js")
                .Include("~/Scripts/Libs/RefreshCountDown/RefreshCountDown.js")
                .Include("~/Scripts/Libs/jquery-ui-1.10.3.custom.js")
                .Include("~/Scripts/Libs/Bootstrap/bootstrap-tooltip.js")
                .Include("~/Scripts/Libs/Bootstrap/bootstrap-popover.js")
                .Include("~/Scripts/Charts/Charts.js"));

            bundles.Add(new ScriptBundle("~/bundles/newchartcontent")
                .Include("~/Scripts/Libs/HighChart/highcharts.js")
                .Include("~/Scripts/Libs/RefreshCountDown/RefreshCountDown.js")
                .Include("~/Scripts/Libs/Bootstrap/bootstrap-dropdown.js")
                .Include("~/Scripts/Charts/NewChartContent.js"));

            bundles.Add(new ScriptBundle("~/bundles/chartmainpagejs")
                .Include("~/Scripts/Charts/MainPage.js"));

            bundles.Add(new ScriptBundle("~/bundles/opensoon")
                .Include("~/Scripts/LIVECHARTS.js")
                .Include("~/Scripts/Libs/CountDown/jquery.countdown.js")
                .Include("~/Scripts/Charts/OpenSoon.js"));
        }

        public static void RegisterChatBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/chatroomjs")
                .Include("~/Scripts/Libs/Moment/moment.js")
                .Include("~/Scripts/Libs/Bootstrap/bootstrap-modal.js")
                .Include("~/Scripts/Libs/PowerTip/jquery.powertip.js")
                .Include("~/Scripts/Libs/Emoticon/jquery.emoticons.js")
                .Include("~/Scripts/Libs/ProfanityFilter/swearWords.js")
                .Include("~/Scripts/Libs/ProfanityFilter/jquery.profanityfilter.js")
                .Include("~/Scripts/Chat/ChatSignalRAdapter.js")
                .Include("~/Scripts/Chat/ChatServer.js")
                .Include("~/Scripts/Chat/ChatClient.js")
                .Include("~/Scripts/Chat/ChatWindow.js"));
        }

        public static void RegisterScorePredictionBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scorepredictionjs")
                .Include("~/Scripts/Libs/HighChart/highcharts.js")
                .Include("~/Scripts/Libs/RefreshCountDown/RefreshCountDown.js")
                .Include("~/Scripts/Libs/Spinner/jquery.stepper.js")
                .Include("~/Scripts/ScorePrediction/ScorePrediction.js")
                .Include("~/Scripts/ScorePrediction/ScorePredictionView.js"));

            bundles.Add(new ScriptBundle("~/bundles/scorepredictionresult")
                .Include("~/Scripts/Libs/HighChart/highcharts.js")
                .Include("~/Scripts/Libs/RefreshCountDown/RefreshCountDown.js")
                .Include("~/Scripts/ScorePrediction/ScorePrediction.js")
                .Include("~/Scripts/ScorePrediction/ScorePredictionResultView.js"));
        }
    }
}