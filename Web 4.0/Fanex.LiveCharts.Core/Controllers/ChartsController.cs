// --------------------------------------------------------------------------
// <copyright file="ChartsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.LiveCharts.ViewModel;
    using Fanex.LiveCharts.Web;
    using Fanex.LiveCharts.Web.Filters;
    using Fanex.Logging;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Chart Controller.
    /// </summary>
    [LiveChartAuthentication]
    public class ChartsController : BaseController
    {
        private readonly IChartService _chartService;
        private readonly IScorePredictionService _scorePredictionService;
        private readonly IMatchService _matchService;

        public ChartsController()
            : this(null, null, null)
        {
        }

        public ChartsController(
            IChartService chartService,
            IScorePredictionService scorePredictionService,
            IMatchService matchService)
        {
            _chartService = chartService ?? new ChartService();
            _scorePredictionService = scorePredictionService ?? new ScorePredictionService();
            _matchService = matchService ?? new MatchService();
        }

        /// <summary>
        /// Get Main page contains chart, score prediction, chat.
        /// </summary>
        /// <param name="matchId">Current match id for get chart.</param>
        /// <returns>Main page.</returns>
        public ActionResult MainPage(int matchId)
        {
            var mainPageViewModel = new MainPageViewModel
            {
                MatchId = matchId,
                AnonymousLogin = SessionManager.CurrentSession.AdminId == 0
                                && SessionManager.CurrentSession.MemberId <= 0,
                IsMatchCompleted = _matchService.IsMatchCompleted(new GetMatchStatusCriteria
                {
                    MatchId = matchId
                })
            };

            return View(mainPageViewModel);
        }

        public ActionResult AnonymousPage()
        {
            return View();
        }

        [NapSuperAuth]
        public ActionResult Index()
        {
            return RedirectToRoute(new
            {
                Area = "Admin",
                Controller = "Match",
                Action = "GetEnableCharts"
            });
        }

        /// <summary>
        /// Index Page, load chart Page.
        /// </summary>
        /// <returns>Index page.</returns>
        ////[NapSuperAuth]
        ////public ActionResult Index()
        ////{
        ////    var currentDate = DateTime.Now;
        ////    var leagues = _chartService.GetLeagues(new GetLeaguesCriteria
        ////        {
        ////            MatchDate = currentDate
        ////        });
        ////    ViewBag.MatchDate = currentDate;
        ////    ViewBag.Leagues = leagues;

        ////    if (leagues != null && leagues.Any())
        ////    {
        ////        var matches = _chartService.GetMatches(new GetMatchesCriteria
        ////        {
        ////            LeagueId = leagues.ElementAt(0).Id,
        ////            MatchDate = ViewBag.MatchDate
        ////        });
        ////        ViewBag.Matches = matches;
        ////        if (matches != null && matches.Any())
        ////        {
        ////            var odds = _chartService.GetOdds(new GetOddsCriteria
        ////            {
        ////                MatchId = matches.ElementAt(0).Id,
        ////                BetTypeId = (int)BetTypeEnum.Handicap
        ////            });
        ////            ViewBag.Odds = odds;
        ////        }
        ////    }

        ////    return View();
        ////}

        /// <summary>
        /// Chart Content page, plug-in member site, will remove check box live.
        /// </summary>
        /// <param name="matchId">Match monitor.</param>
        /// <param name="betTypeId">Current bet type.</param>
        /// <param name="teamChoosing">Team choosing, home or away.</param>
        /// <returns>Return View.</returns>
        [LiveChartAuthentication]
        public ActionResult ChartContent(
            int matchId,
            int betTypeId = (int)BetTypeEnum.Handicap,
            int teamChoosing = (int)TeamChoosingType.Home)
        {
            if (SessionManager.IsValid())
            {
                ////if (SessionManager.CurrentSession.AdminId == 0
                ////    && SessionManager.CurrentSession.MemberId > 0
                ////    && SessionManager.CurrentSession.MatchId != matchId)
                ////{
                ////    return RedirectToAction("ShowOnlyOneMatch", "User");
                ////}

                var chartContentViewModel = GetChartContentViewModel(
                    matchId,
                    betTypeId,
                    teamChoosing,
                    false);

                if (chartContentViewModel.OpenSoonCountdown > 0)
                {
                    return View("~/Views/Charts/OpenSoon.cshtml", chartContentViewModel);
                }
                else
                {
                    return View("~/Views/Charts/NewChartContent.cshtml", chartContentViewModel);
                }
            }

            return RedirectToAction("Unauthorized", "User");
        }

        /////// <summary>
        /////// Chart content show in internal site.
        /////// </summary>
        /////// <param name="matchId">Current Monitor match.</param>
        /////// <param name="betTypeId">Current bet type.</param>
        /////// <param name="teamChoosing">Current team choosing.</param>
        /////// <returns>Chart content frame.</returns>
        ////public ActionResult ChartContentInternal(
        ////    int matchId,
        ////    int betTypeId = (int)BetTypeEnum.Handicap,
        ////    int teamChoosing = (int)TeamChoosingType.Home)
        ////{
        ////    var chartContentViewModel = GetChartContentViewModel(
        ////        matchId,
        ////        betTypeId,
        ////        teamChoosing,
        ////        true);

        ////    if (chartContentViewModel.OpenSoonCountdown > 0)
        ////    {
        ////        return View("~/Views/Charts/OpenSoon.cshtml", chartContentViewModel);
        ////    }
        ////    else
        ////    {
        ////        return View("~/Views/Charts/NewChartContent.cshtml", chartContentViewModel);
        ////    }
        ////}

        /// <summary>
        /// Get Chart content view model for showing data.
        /// </summary>
        /// <param name="matchId">Match Id for Get data.</param>
        /// <param name="betTypeId">Current bet type id.</param>
        /// <param name="teamChoosing">Current team choice.</param>
        /// <param name="showLiveAndDeadball">Show all live and dead ball.</param>
        /// <returns>Chart content view model.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public ChartContentViewModel GetChartContentViewModel(
            int matchId,
            int betTypeId = (int)BetTypeEnum.Handicap,
            int teamChoosing = (int)TeamChoosingType.Home,
            bool showLiveAndDeadball = false)
        {
            var match = _chartService.GetMatchDetail(new GetMatchDetailCriteria
            {
                MatchId = matchId
            });

            //// Hard code just only show Live
            var openSoonCountDown = 0f;
            var isLive = _chartService.CurrentMatchIsLive(matchId, match.KickOffTime, ref openSoonCountDown);

            var handicapOddList = _chartService.GetOdds(new GetOddsCriteria
            {
                BetTypeId = (int)BetTypeEnum.Handicap,
                MatchId = matchId,
                IsLive = true
            }).Where(o => o.ChartStatus);

            var overUnderOddList = _chartService.GetOdds(new GetOddsCriteria
            {
                BetTypeId = (int)BetTypeEnum.OverUnder,
                MatchId = matchId,
                IsLive = true
            }).Where(o => o.ChartStatus);

            if (match != null)
            {
                return new ChartContentViewModel
                {
                    BetTypeId = betTypeId,
                    IsLive = isLive,
                    HandicapOddsList = handicapOddList,
                    OverUnderOddsList = overUnderOddList,
                    TeamChoosing = teamChoosing,
                    MatchId = matchId,
                    KickOffTime = match.KickOffTime,
                    HomeId = match.HomeId,
                    AwayId = match.AwayId,
                    ShowLiveAndDeadball = showLiveAndDeadball,
                    IsFirefox = Request.Browser.Browser.ToLower().Contains("firefox"),
                    IsChrome = Request.Browser.Browser.ToLower().Contains("chrome"),
                    ShowHandicap = handicapOddList.Any(),
                    ShowOverUnder = overUnderOddList.Any(),
                    OpenSoonCountdown = openSoonCountDown
                };
            }
            else
            {
                return new ChartContentViewModel();
            }
        }

        /// <summary>
        /// Keep alive action.
        /// </summary>
        /// <returns>Check alive.</returns>
        [HttpGet]
        public ActionResult KeepAlive()
        {
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Information for showing Handicap or Over Under.
        /// </summary>
        /// <param name="matchId">Current match id.</param>
        /// <param name="isLive">Current match is live or not.</param>
        /// <returns>Flag for showing handicap and over under.</returns>
        [HttpGet]
        public JsonResult ShowBetType(int matchId = 0, bool isLive = true)
        {
            var showHandicap = _chartService.GetOdds(new GetOddsCriteria
            {
                BetTypeId = (int)BetTypeEnum.Handicap,
                MatchId = matchId,
                IsLive = isLive
            }).Where(o => o.ChartStatus).Any();

            var showOverUnder = _chartService.GetOdds(new GetOddsCriteria
            {
                BetTypeId = (int)BetTypeEnum.OverUnder,
                MatchId = matchId,
                IsLive = isLive
            }).Where(o => o.ChartStatus).Any();

            return Json(
                new
            {
                showHandicap = showHandicap,
                showOverUnder = showOverUnder
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsMatchCompleted(GetMatchStatusCriteria getMatchStatusCriteria)
        {
            var isCompleted = _matchService.IsMatchCompleted(getMatchStatusCriteria);

            return Json(isCompleted);
        }

        public ActionResult OpenSoon()
        {
            return View();
        }

        /// <summary>
        /// Test Method.
        /// </summary>
        /// <param name="updateCache">Update cache or not.</param>
        /// <returns>Test page.</returns>
        public ActionResult Test(bool updateCache = false)
        {
            var testKey = "demoTestCache";
            var chart = CacheManager.Instance.Get<Chart>(testKey);
            if (chart == null && updateCache)
            {
                chart = new Chart
                {
                    BetTypeId = 1,
                    CurrentMatch = new Match
                    {
                        AwayId = 1,
                        HomeId = 1,
                    },
                    OddsId = 1,
                };
                CacheManager.Instance.Set<Chart>(testKey, chart, new TimeSpan(0, 0, 20));
            }

            return View(chart);
        }

        public ActionResult TestLogging()
        {
            Logger.Log.Error("Test Logging!");
            return new EmptyResult();
        }
    }
}