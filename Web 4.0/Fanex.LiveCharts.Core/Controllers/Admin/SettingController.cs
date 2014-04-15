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
    using Fanex.Common;
    using Fanex.LiveCharts.Criterias.Chat;
    using Fanex.LiveCharts.Criterias.ScorePrediction;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.LiveCharts.ViewModel.Chat;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Data;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [NapSuperAuth]
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
            var scorePredictionSetting = _settingService.GetSetting(new ChartAdminGetMinuteForClosingPredictionCriteria());
            return View(scorePredictionSetting);
        }

        [HttpPost]
        public ActionResult ScorePrediction(SettingItem settingItem)
        {
            ViewBag.updateResult = _settingService.UpdateSetting(new ChartAdminUpdateMinuteForClosingPredictionCriteria
                {
                    ClosePredictionMinute = Converter.ToInt(settingItem.SettingValue)
                });
            return View(settingItem);
        }

        [HttpGet]
        public ActionResult Chat()
        {
            ChatSettingViewModel chatSetting = new ChatSettingViewModel
            {
                TermCondition = _settingService.GetSetting(new GetTermConditionCriteria()),
                WelcomeMessage = _settingService.GetSetting(new GetWelcomeMessageCriteria())
            };

            return View(chatSetting);
        }

        [HttpPost]
        public JsonResult UpdateTermCondition(string termCondition, bool disabled)
        {
            ICommandResult updateResult = _settingService.UpdateSetting(new UpdateTermConditionCriteria
            {
                TermCondition = termCondition,
                Disabled = disabled
            });

            return Json(updateResult);
        }

        [HttpPost]
        public JsonResult UpdateWelcomeMessage(string welcomeMessage, bool disabled)
        {
            ICommandResult updateResult = _settingService.UpdateSetting(new UpdateWelcomeMessage
            {
                WelcomeMessage = welcomeMessage,
                Disabled = disabled
            });

            return Json(updateResult);
        }
    }
}