// --------------------------------------------------------------------------
// <copyright file="IntroductionController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Controllers
{
    using System.Web.Mvc;
    using Fanex.LiveCharts.Web;
    using Fanex.LiveCharts.Web.Filters;

    /// <summary>
    /// Introduction controller for help page.
    /// </summary>
    [ReCheckIn]
    public class IntroductionController : BaseController
    {
        /// <summary>
        /// Chart help page.
        /// </summary>
        /// <param name="key">CSHTML file name.</param>
        /// <param name="title">Page title.</param>
        /// <returns>Introduction View.</returns>
        public ActionResult Index(string key, string title = "")
        {
            ViewBag.Title = title;
            var language = SessionManager.CurrentSession.Language;
            switch (language)
            {
                case "zh-TW":
                case "zh-CN":
                    break;
                default:
                    language = "en-US";
                    break;
            }

            string viewName = string.Format("~/Views/Introduction/{0}/{1}.cshtml", language, key);

            return View(viewName);
        }
    }
}