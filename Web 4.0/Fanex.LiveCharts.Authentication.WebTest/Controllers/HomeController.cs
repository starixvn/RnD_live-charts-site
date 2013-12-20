// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Authentication.WebTest.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.LiveCharts.ClientAuthentication;
    using Fanex.LiveCharts.ClientAuthentication.Core;

    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Load charts.
        /// </summary>
        /// <param name="language">Current Language.</param>
        /// <returns>Current View.</returns>
        public ActionResult LoadChart(string language = "en-US")
        {
            var currentLanguage = LanguageType.English;
            switch (language)
            {
                case "zh-TW":
                    currentLanguage = LanguageType.ChineseTraditional;
                    break; // Traditional
                case "zh-CN":
                    currentLanguage = LanguageType.ChineseSimplified;
                    break; // Simplified
                case "ja-JP":
                    currentLanguage = LanguageType.Japanese;
                    break;
                case "th-TH":
                    currentLanguage = LanguageType.Thai;
                    break; // Thai
                case "ko-KR":
                    currentLanguage = LanguageType.Korean;
                    break; // Korean
                case "vi-VN":
                    currentLanguage = LanguageType.Vietnamese;
                    break; // Vietnamese
            }

            var urlAccess = LiveChartsTokenService.GetAccessUrl(1, 4888888, (int)currentLanguage);
            ////Thread.Sleep(58000);
            return Redirect(urlAccess);
        }

        public ActionResult LoadMatch()
        {
            return View(LiveChartsTokenService.GetCharts(new DateTime(2013, 7, 16)));
        }
    }
}