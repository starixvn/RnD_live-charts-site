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
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.LiveCharts.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScorePredictionController : BaseController
    {
        private const string CorePredictionCookie = "E86E0D7F-FD5A-412B-823F-A4E6517AE51D";
        private IScorePredictionService _scorePredictionService;


        public ScorePredictionController()
            : this(null)
        {
        }

        public ScorePredictionController(IScorePredictionService scorePredictionService)
        {
            _scorePredictionService = scorePredictionService ?? new ScorePredictionService();
        }

        #region Member Site

        public ActionResult Index(int matchId, string homeName, string awayName, bool isAdminSite = false)
        {
            return View(new ScorePredictionResultViewModel
            {
                MatchId = matchId,
                HomeName = homeName,
                AwayName = awayName,
                IsAdminSite = isAdminSite
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
            criteria.MemberId = GetUserIdFromCookie(criteria.MemberId, criteria.MatchId);

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
            getUserPredictionCriteria.MemberId = GetUserIdFromCookie(getUserPredictionCriteria.MemberId, getUserPredictionCriteria.MatchId);

            var result = _scorePredictionService.GetUserPrediction(getUserPredictionCriteria);
            return Json(result);
        }

        /// <summary>
        /// Get User Id from Cookie if current user is not member.
        /// </summary>
        /// <param name="memberId">Current member id, if it's less than 0, then it's not member.</param>
        /// <param name="matchId">Current match id for score prediction.</param>
        /// <returns>String contain member identity.</returns>
        [NonAction]
        private string GetUserIdFromCookie(string memberId, int matchId)
        {
            var currentMemberId = Converter.ToInt(memberId);

            if (currentMemberId < 0)
            {
                var cookieName = string.Format("{0}_{1}", CorePredictionCookie, matchId);
                var currentCookie = HttpContext.Request.Cookies[cookieName];
                if (currentCookie != null)
                {
                    return currentCookie.Value;
                }
                else
                {
                    var newMemberId = Guid.NewGuid().ToString();
                    var scorePredictionCookie = new HttpCookie(cookieName, newMemberId);
                    scorePredictionCookie.Expires = DateTime.Now.AddDays(1);
                    HttpContext.Response.SetCookie(scorePredictionCookie);
                }
            }

            return memberId;
        }

        #endregion Member Site

        #region Admin Site

        /// <summary>
        /// Admin page for score prediction configuration.
        /// </summary>
        /// <param name="matchId">Current match id for configuration.</param>
        /// <param name="homeName">Current home name.</param>
        /// <param name="awayName">Current away name.</param>
        /// <returns>Return view.</returns>
        [NapSuperAuth]
        public ActionResult AdminPage(int matchId, string homeName, string awayName)
        {
            ViewBag.ScorePredictionOptions = _scorePredictionService.GetTotalScorePrediction(new GetAllScorePredictionCriteria
            {
                IsAdminSite = true,
                MatchId = matchId
            });

            return View(
                "~/Areas/Admin/Views/ScorePrediction/ScorePredictionMonitor.cshtml",
                new ScorePredictionResultViewModel
                {
                    MatchId = matchId,
                    HomeName = homeName,
                    AwayName = awayName,
                    IsAdminSite = true
                });
        }

        [NapSuperAuth, HttpPost]
        public JsonResult UpdateDummyPrediction(ChartAdminInsertUpdateDummyPredictionCriteria updateCriteria)
        {
            updateCriteria.AdminId = SessionManager.CurrentSession.AdminId;
            updateCriteria.AdminName = SessionManager.CurrentSession.AdminName;
            var result = _scorePredictionService.InsertOrUpdateDummyPrediction(updateCriteria);
            return Json(result);
        }

        [NapSuperAuth, HttpGet]
        public ActionResult GetScorePredictionLogTable(int matchId)
        {
            var result = _scorePredictionService.GetTotalScorePrediction(new GetAllScorePredictionCriteria
                {
                    IsAdminSite = true,
                    MatchId = matchId
                });

            return View("~/Areas/Admin/Views/ScorePrediction/ScorePredictionLogTablePartial.cshtml", result.ScorePredictionOptions);
        }


        #endregion Admin Site
    }
}