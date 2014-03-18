// --------------------------------------------------------------------------
// <copyright file="SettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.18">Created</change>
// </history>

namespace Fanex.LiveCharts.Controllers.Admin
{
    using System.Web.Mvc;
    using Fanex.LiveCharts.Criterias.ScorePrediction;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingController()
            : this(null)
        {
        }

        public SettingController(SettingService settingService)
        {
            _settingService = settingService ?? new SettingService();
        }

        [HttpGet]
        public ActionResult ScorePrediction()
        {
            var settingItem = _settingService.GetClosePredictionMinuteSetting(new ChartAdminGetMinuteForClosingPredictionCriteria());
            return View(settingItem);
        }

        [HttpPost]
        public ActionResult ScorePrediction(int availableTime)
        {
            var updateResult = _settingService.UpdateClosePredictionMinuteSetting(new ChartAdminUpdateMinuteForClosingPredictionCriteria
                {
                    ClosePredictionMinute = availableTime
                });
            return View();
        }
    }
}