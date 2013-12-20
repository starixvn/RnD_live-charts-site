// --------------------------------------------------------------------------
// <copyright file="MatchController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.8.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Controllers.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Resources;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Controller handle chart action.
    /// </summary>
    [NapSuperAuth]
    public class MatchController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly IChartService _chartService;

        public MatchController()
            : this(null)
        {
        }

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService ?? new MatchService();
            _chartService = new ChartService();
        }

        /// <summary>
        /// Get Enable Charts.
        /// </summary>
        /// <param name="fromDate">Search from date.</param>
        /// <param name="toDate">Search to date.</param>
        /// <param name="statusId">Chart status.</param>
        /// <returns>List chart match with filter.</returns>
        public ActionResult GetEnableCharts(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            bool statusId = false)
        {
            GetEnableChartsCriteria criteria = new GetEnableChartsCriteria
            {
                StatusId = statusId,
                FromDate = fromDate ?? DateTime.Now.AddDays(-7),
                ToDate = toDate ?? DateTime.Now
            };
            var matches = _matchService.GetEnableCharts(criteria);
            ViewBag.Matches = matches;
            return View(criteria);
        }

        /// <summary>
        /// Get Enable Chart.
        /// </summary>
        /// <param name="criteria">Criteria filter.</param>
        /// <returns>List charts.</returns>
        [HttpPost]
        public ActionResult GetEnableCharts(GetEnableChartsCriteria criteria)
        {
            return GetEnableCharts(criteria.FromDate, criteria.ToDate, criteria.StatusId);
        }

        /// <summary>
        /// Get match with match id.
        /// </summary>
        /// <param name="id">Match need get information.</param>
        /// <returns>Match result.</returns>
        public ActionResult GetMatch(int id)
        {
            GetMatchByIdCriteria criteria = new GetMatchByIdCriteria
            {
                MatchId = id
            };
            var match = _matchService.GetMatchById(criteria);
            ViewBag.OddsGroup = _chartService.GetOddsGroup(new GetOddsCriteria
            {
                MatchId = id,
                IsLive = true
            });
            return View(match);
        }

        /// <summary>
        /// Search match.
        /// </summary>
        /// <param name="date">Event date.</param>
        /// <param name="sportId">Sport type.</param>
        /// <param name="leagueId">League Id.</param>
        /// <param name="keyword">Search key work.</param>
        /// <param name="kickedOff">Show kicked off match or not.</param>
        /// <returns>List match result.</returns>
        public ActionResult Search(
            DateTime? date = null,
            int sportId = 1,
            int leagueId = 0,
            string keyword = null,
            bool kickedOff = true)
        {
            SearchMatchesCriteria criteria = new SearchMatchesCriteria
            {
                MatchDate = date ?? DateTime.Now.Date,
                SportId = sportId,
                LeagueId = leagueId,
                Keyword = keyword,
                KickedOff = kickedOff
            };

            ////Apply for Soccer only
            var soccer = CacheRef.GetSportName(1);
            ViewBag.Soccer = soccer;
            ViewBag.Leagues = GetLeaguesByDate(criteria.MatchDate);

            var matches = _matchService.SearchMatches(criteria);
            ViewBag.MatchList = matches;
            return View(criteria);
        }

        /// <summary>
        /// Search match post action.
        /// </summary>
        /// <param name="criteria">Get match criteria.</param>
        /// <returns>Match result.</returns>
        [HttpPost]
        public ActionResult Search(SearchMatchesCriteria criteria)
        {
            return Search(criteria.MatchDate, criteria.SportId, criteria.LeagueId, criteria.Keyword);
        }

        /// <summary>
        /// Import match to match queue for monitoring.
        /// </summary>
        /// <param name="importMatchQueueCriteria">Import match criteria.</param>
        /// <returns>Update result.</returns>
        [HttpPost]
        public JsonResult ImportMatchQueue(ImportMatchQueueCriteria importMatchQueueCriteria)
        {
            return Json(_matchService.ImportMatchQueue(importMatchQueueCriteria));
        }

        /// <summary>
        /// Get match list.
        /// </summary>
        /// <param name="date">Event date.</param>
        /// <param name="sportId">Sport Type.</param>
        /// <param name="leagueId">League Id.</param>
        /// <param name="keyword">With key work.</param>
        /// <param name="kickedOff">Show kicked off match or not.</param>
        /// <returns>Match list view result.</returns>
        public ActionResult MatchList(
            DateTime? date = null,
            int sportId = 1,
            int leagueId = 0,
            string keyword = null,
            bool kickedOff = false)
        {
            SearchMatchesCriteria criteria = new SearchMatchesCriteria
            {
                MatchDate = date ?? DateTime.Now.Date,
                SportId = sportId,
                LeagueId = leagueId,
                Keyword = keyword,
                KickedOff = kickedOff
            };

            var matches = _matchService.SearchMatches(criteria);
            return View(matches);
        }

        /// <summary>
        /// Get league with date and sport type.
        /// </summary>
        /// <param name="date">Event date.</param>
        /// <param name="sportId">Sport Type.</param>
        /// <returns>List league.</returns>
        [HttpPost]
        public JsonResult GetLeagues(DateTime? date, int sportId = 1)
        {
            return Json(GetLeaguesByDate(date, sportId));
        }

        /// <summary>
        /// Update odds status for monitoring.
        /// </summary>
        /// <param name="oddsEnableList">Odds list need be enabled.</param>
        /// <param name="oddsDisableList">Odds list need be disabled.</param>
        /// <returns>Success or not.</returns>
        [HttpPost]
        public JsonResult UpdateOddsListStatus(string oddsEnableList = "", string oddsDisableList = "")
        {
            ////UpdateOddsStatusCriteria criteria
            ICommandResult result = null;
            if (!string.IsNullOrEmpty(oddsEnableList))
            {
                var listEnableOdds = oddsEnableList.Split(',');
                foreach (var odds in listEnableOdds)
                {
                    result = _matchService.UpdateOddsStatus(new UpdateOddsStatusCriteria
                    {
                        OddsId = Converter.ToInt(odds),
                        EnableChart = true
                    });

                    if (result.Code > 0)
                    {
                        return Json(result);
                    }
                }
            }

            if (!string.IsNullOrEmpty(oddsDisableList))
            {
                var listDisableOdds = oddsDisableList.Split(',');
                foreach (var odds in listDisableOdds)
                {
                    result = _matchService.UpdateOddsStatus(new UpdateOddsStatusCriteria
                    {
                        OddsId = Converter.ToInt(odds),
                        EnableChart = false
                    });

                    if (result.Code > 0)
                    {
                        return Json(result);
                    }
                }
            }

            return Json(result);
        }

        /// <summary>
        /// Get odds list for update Odds status.
        /// </summary>
        /// <param name="getOddsCriteria">Get Odds criteria.</param>
        /// <returns>Odds List.</returns>
        [HttpPost]
        public JsonResult GetOddsList(GetOddsCriteria getOddsCriteria)
        {
            var oddsList = _chartService.GetOdds(getOddsCriteria);
            return Json(oddsList);
        }

        [HttpPost]
        public JsonResult UpdateChartShowingStatus(UpdateChartShowingStatusCriteria updateChartShowingStatus)
        {
            var updateResult = _matchService.UpdateChartShowingStatus(updateChartShowingStatus);
            return Json(updateResult);
        }

        /// <summary>
        /// Get League by date.
        /// </summary>
        /// <param name="date">Event date.</param>
        /// <param name="sportId">Sport Type.</param>
        /// <returns>League list.</returns>
        private IEnumerable<SelectListItem> GetLeaguesByDate(DateTime? date, int sportId = 1)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            GetLeaguesAdminCriteria getLeaguesCriteria = new GetLeaguesAdminCriteria
            {
                MatchDate = date ?? DateTime.Now.Date,
                SportId = sportId
            };

            var leagues = _matchService.GetLeagues(getLeaguesCriteria)
                          .OrderBy(l => l.LeagueName)
                          .Select(league => new SelectListItem
                          {
                              Text = league.LeagueName,
                              Value = league.Id.ToString()
                          });

            if (leagues != null
                && leagues.Count() > 0)
            {
                result.Add(new SelectListItem() { Value = 0.ToString(), Text = AdminSiteLang.AllLeagues });
                result.AddRange(leagues);
            }

            return result;
        }
    }
}