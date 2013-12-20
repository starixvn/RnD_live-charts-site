// --------------------------------------------------------------------------
// <copyright file="ChartApiController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.LiveCharts.Web.Filters;

    /// <summary>
    /// Movement Chart for get data.
    /// </summary>
    [Authorize]
    public class ChartApiController : ApiController
    {
        private readonly IChartService _chartService;

        public ChartApiController()
            : this(null)
        {
        }

        public ChartApiController(IChartService chartService)
        {
            _chartService = chartService ?? new ChartService();
        }

        /// <summary>
        /// Get chart information in chart initial.
        /// </summary>
        /// <param name="matchId">Match id for get chart.</param>
        /// <param name="betTypeId">Bet type id.</param>
        /// <param name="oddsIds">Odds id list for get data.</param>
        /// <param name="isLive">Match live or not.</param>
        /// <param name="kickOffTime">Match kick off time.</param>
        /// <returns>Charts list.</returns>
        [HttpGet]
        public IEnumerable<Chart> GetCharts(
            int matchId,
            int betTypeId,
            string oddsIds,
            bool isLive,
            DateTime? kickOffTime)
        {
            IEnumerable<Chart> result = _chartService.GetCharts(new ChartFilter
            {
                BetTypeId = betTypeId,
                IsLive = isLive,
                KickOffTime = (DateTime)kickOffTime,
                MatchId = matchId,
                OddsIds = oddsIds,
                ChartId = (int)ChartType.All
            });

            return result;
        }

        /// <summary>
        /// Get chart data.
        /// </summary>
        /// <param name="matchId">Current Match id.</param>
        /// <param name="betTypeId">Current bet type id.</param>
        /// <param name="oddsId">Current odds id.</param>
        /// <param name="isLive">Match is live or not.</param>
        /// <param name="kickOffTime">Match kick off time.</param>
        /// <param name="chartId">Current Chart type.</param>
        /// <returns>Chart data.</returns>
        [HttpGet]
        public Chart GetChart(
            int matchId,
            int betTypeId,
            int oddsId,
            bool isLive,
            DateTime? kickOffTime,
            int chartId)
        {
            Chart result = _chartService.GetChart(new ChartFilter
            {
                BetTypeId = betTypeId,
                IsLive = isLive,
                KickOffTime = (DateTime)kickOffTime,
                MatchId = matchId,
                OddsId = oddsId,
                ChartId = chartId
            });
            return result;
        }

        /// <summary>
        /// Get price chart points.
        /// </summary>
        /// <param name="priceChartTimeIndex">Current price chart time index.</param>
        /// <param name="matchId">Match id for get data.</param>
        /// <param name="betTypeId">Current bet type.</param>
        /// <param name="oddsId">Current odds id.</param>
        /// <param name="isLive">Chart live or not.</param>
        /// <param name="kickOffTime">Kick off time of match.</param>
        /// <returns>Point list.</returns>
        [HttpGet, ReCheckIn]
        public Dictionary<int, Point> GetPriceChartPoints(
            int priceChartTimeIndex,
            int matchId,
            int betTypeId,
            int oddsId,
            bool isLive,
            DateTime? kickOffTime)
        {
            var result = _chartService.GetPriceChartPoints(new ChartFilter
            {
                BetTypeId = betTypeId,
                IsLive = isLive,
                KickOffTime = (DateTime)kickOffTime,
                MatchId = matchId,
                OddsId = oddsId,
                PriceOddsChartTimeIndex = priceChartTimeIndex
            });
            return result;
        }

        /// <summary>
        /// Get market point movement.
        /// </summary>
        /// <param name="marketChartTimeIndex">Market chart time index.</param>
        /// <param name="matchId">Current match id.</param>
        /// <param name="betTypeId">Bet type id.</param>
        /// <param name="oddsId">Odds id for get data.</param>
        /// <param name="isLive">Current match is live or not.</param>
        /// <param name="kickOffTime">Current match kick off time.</param>
        /// <returns>Point list.</returns>
        [HttpGet]
        public Dictionary<int, Point> GetMarketChartPoints(
            int marketChartTimeIndex,
            int matchId,
            int betTypeId,
            int oddsId,
            bool isLive,
            DateTime? kickOffTime)
        {
            var result = _chartService.GetMarketChartPoints(new ChartFilter
                {
                    BetTypeId = betTypeId,
                    IsLive = isLive,
                    KickOffTime = (DateTime)kickOffTime,
                    MarketChartTimeIndex = marketChartTimeIndex,
                    MatchId = matchId,
                    OddsId = oddsId
                });
            return result;
        }

        /// <summary>
        /// Get Match list.
        /// </summary>
        /// <param name="leagueId">Current league id.</param>
        /// <param name="matchDate">Match date for searching.</param>
        /// <returns>Match list.</returns>
        [HttpGet]
        public IEnumerable<Match> GetMatches(int leagueId, DateTime matchDate)
        {
            var result = _chartService.GetMatches(new GetMatchesCriteria
                {
                    LeagueId = leagueId,
                    MatchDate = matchDate
                });
            return result;
        }

        /// <summary>
        /// Get Odds list with bet type, match id and live indicator.
        /// </summary>
        /// <param name="betTypeId">Current bet type.</param>
        /// <param name="matchId">Match id for get data.</param>
        /// <param name="isLive">Match live indicator.</param>
        /// <returns>Odds list.</returns>
        [HttpGet]
        public IEnumerable<Odds> GetOdds(int betTypeId, int matchId, bool isLive)
        {
            var result = _chartService.GetOdds(new GetOddsCriteria
                {
                    BetTypeId = betTypeId,
                    MatchId = matchId,
                    IsLive = isLive
                }).Where(o => o.ChartStatus);
            return result;
        }

        /// <summary>
        /// Get League in date.
        /// </summary>
        /// <param name="matchDate">Current match date.</param>
        /// <returns>League list.</returns>
        [HttpGet]
        public IEnumerable<League> GetLeagues(DateTime matchDate)
        {
            var result = _chartService.GetLeagues(new GetLeaguesCriteria
                {
                    MatchDate = matchDate
                });
            return result;
        }

        /// <summary>
        /// Get list indicator points.
        /// </summary>
        /// <param name="matchId">Current match id.</param>
        /// <param name="eventChartTimeIndex">Current max time index of event chart.</param>
        /// <param name="oddsId">Current odds id.</param>
        /// <param name="isLive">Current match id live or not.</param>
        /// <param name="betTypeId">Current bet type id for query database.</param>
        /// <param name="kickOffTime">Current match kick off time.</param>
        /// <returns>List indicator points.</returns>
        [HttpGet]
        public Dictionary<int, Point> GetIndicatorPoints(
            int matchId,
            int eventChartTimeIndex,
            int oddsId,
            bool isLive,
            int betTypeId,
            DateTime? kickOffTime)
        {
            Dictionary<int, Point> result = _chartService.GetEventChartPoints(new ChartFilter
            {
                IsLive = isLive,
                KickOffTime = (DateTime)kickOffTime,
                MatchId = matchId,
                OddsId = oddsId,
                EventChartTimeIndex = eventChartTimeIndex,
                BetTypeId = betTypeId
            });

            return result;
        }

        /// <summary>
        /// Get Match Detail with match id.
        /// </summary>
        /// <param name="matchId">Current match id.</param>
        /// <returns>Match detail information.</returns>
        [HttpGet]
        public string GetKickOffTime(int matchId)
        {
            var match = _chartService.GetMatchDetail(new GetMatchDetailCriteria
            {
                MatchId = matchId
            });

            if (match != null)
            {
                return match.KickOffTime.ToString();
            }

            return string.Empty;
        }

        [HttpGet]
        public CurrentMatchTime GetCurrentMatchTime(int matchId)
        {
            var currentMatchTime = _chartService.GetCurrentMatchTime(new GetCurrentMatchTimeCriteria
            {
                MatchId = matchId
            });

            return currentMatchTime;
        }
    }
}