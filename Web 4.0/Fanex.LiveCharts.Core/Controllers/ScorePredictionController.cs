// --------------------------------------------------------------------------
// <copyright file="ScorePredictionController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.LiveCharts.Criterias.ScorePrediction;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.LiveCharts.ViewModel.ScorePredictionViewModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScorePredictionController : Controller
    {
        private const string CorePredictionCookie = "Fanex.LiveCharts.Controllers.ScorePredictionController";
        private IScorePredictionService _scorePredictionService;

        public ScorePredictionController()
            : this(null)
        {
        }

        public ScorePredictionController(IScorePredictionService scorePredictionService)
        {
            _scorePredictionService = scorePredictionService ?? new ScorePredictionService();
        }

        public ActionResult Index(int matchId, string homeName, string awayName)
        {
            return View(new ScorePredictionResultViewModel
            {
                MatchId = matchId,
                HomeName = homeName,
                AwayName = awayName
            });
        }

        [HttpPost]
        public JsonResult GetScorePrictionResult(GetAllScorePredictionCriteria getAllPredictionCriteria)
        {
            var result = _scorePredictionService.GetTotalScorePrediction(getAllPredictionCriteria);
            return Json(result);
        }

        /// <summary>
        /// Insert or update user prediction, if anonymous then create new id then set to cookie.
        /// </summary>
        /// <param name="criteria">Parameters to update/insert to database.</param>
        /// <returns>JSON result information.</returns>
        [HttpPost]
        public JsonResult InsertOrUpdateScorePrediction(InsertOrUpdateScorePredictionItemCriteria criteria)
        {
            var currentMemberId = Converter.ToInt(criteria.MemberId);
            if (currentMemberId < 0)
            {
                criteria.MemberId = Guid.NewGuid().ToString();
                HttpContext.Response.SetCookie(new HttpCookie(CorePredictionCookie, criteria.MemberId));
            }

            var result = _scorePredictionService.InsertOrUpdateScorePredictionItem(criteria);
            return Json(result);
        }

        /// <summary>
        /// Get current user prediction.
        /// </summary>
        /// <param name="getUserPredictionCriteria">Parameter for get data.</param>
        /// <returns>User prediction information.</returns>
        [HttpPost]
        public JsonResult GetUserPrediction(GetUserPredictionCriteria getUserPredictionCriteria)
        {
            var currentMemberId = Converter.ToInt(getUserPredictionCriteria.MemberId);

            if (currentMemberId < 0)
            {
                var currentCookie = HttpContext.Request.Cookies[CorePredictionCookie];
                if (currentCookie != null)
                {
                    getUserPredictionCriteria.MemberId = currentCookie.Value;
                }
            }

            var result = _scorePredictionService.GetUserPrediction(getUserPredictionCriteria);
            return Json(result);
        }
    }
}