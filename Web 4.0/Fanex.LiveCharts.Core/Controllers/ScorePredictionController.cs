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
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Criterias.ScorePrediction;
    using Fanex.LiveCharts.Resources;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.LiveCharts.ViewModel.ScorePrediction;
    using Fanex.LiveCharts.Web;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScorePredictionController : BaseController
    {
        private const string CorePredictionCookie = "E86E0D7F-FD5A-412B-823F-A4E6517AE51D";
        private IScorePredictionService _scorePredictionService;
        private IChartService _chartService;

        public ScorePredictionController()
            : this(null, null)
        {
        }

        public ScorePredictionController(IScorePredictionService scorePredictionService, IChartService chartService)
        {
            _scorePredictionService = scorePredictionService ?? new ScorePredictionService();
            _chartService = chartService ?? new ChartService();
        }

        #region Member Site

        /// <summary>
        /// Index page for score prediction.
        /// </summary>
        /// <param name="matchId">Prediction for match id.</param>
        /// <returns>View of score prediction page.</returns>
        public ActionResult Index(int matchId)
        {
            var match = _chartService.GetMatchDetail(new GetMatchDetailCriteria
            {
                MatchId = matchId
            });

            return View(new ScorePredictionViewModel
            {
                Match = match,
                IsStopScorePrediction = _scorePredictionService.IsStopScorePrediction(matchId),
                MemberId = SessionManager.CurrentSession.MemberId
            });
        }

        public ActionResult ResultPage(int matchId, string homeName, string awayName, bool isAdminSite = false)
        {
            return View(new ScorePredictionResultViewModel
            {
                MatchId = matchId,
                HomeName = homeName,
                AwayName = awayName,
                IsAdminSite = isAdminSite,
                IsStopScorePrediction = _scorePredictionService.IsStopScorePrediction(matchId)
            });
        }

        [HttpPost]
        public JsonResult GetScorePrictionResult(GetAllScorePredictionCriteria getAllPredictionCriteria)
        {
            var result = _scorePredictionService.GetTotalScorePrediction(getAllPredictionCriteria);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetScorePredictionOption(GetScorePredictionOptionCriteria criteria)
        {
            return Json(_scorePredictionService.GetScorePredictionOption(criteria));
        }

        /// <summary>
        /// Insert or update user prediction, if anonymous then create new id then set to cookie.
        /// </summary>
        /// <param name="criteria">Parameters to update/insert to database.</param>
        /// <returns>JSON result information.</returns>
        [HttpPost]
        public JsonResult InsertOrUpdateScorePrediction(InsertOrUpdateScorePredictionItemCriteria criteria)
        {
            if (!_scorePredictionService.IsStopScorePrediction(criteria.MatchId))
            {
                criteria.MemberId = GetUserIdFromCookie(criteria.MemberId, criteria.MatchId);

                var result = _scorePredictionService.InsertOrUpdateScorePredictionItem(criteria);
                return Json(result);
            }
            else
            {
                return Json(new CommandResult
                {
                    Code = 1,
                    Message = Lang.ScorePredictionStopMessage
                });
            }
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
        /// Get Current Match Status for closing score prediction.
        /// </summary>
        /// <param name="matchId">Current Match Id.</param>
        /// <returns>Stop score prediction decision.</returns>
        [HttpPost]
        public JsonResult CheckStopScorePrediction(int matchId)
        {
            return Json(new
            {
                IsStopScorePrediction = _scorePredictionService.IsStopScorePrediction(matchId)
            });
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

        /// <summary>
        /// Score Prediction monitor page.
        /// </summary>
        /// <param name="matchId">Current match id for monitoring.</param>
        /// <param name="homeName">Home name.</param>
        /// <param name="awayName">Away name.</param>
        /// <returns>Score prediction view.</returns>
        [NapSuperAuth]
        public ActionResult ScorePredictionMonitor(int matchId, string homeName, string awayName)
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
    }
}