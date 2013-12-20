// --------------------------------------------------------------------------
// <copyright file="IChartService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Models;

    /// <summary>
    /// I Chart service.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public interface IChartService
    {
        IEnumerable<Chart> GetCharts(ChartFilter chartFilter);

        Chart GetChart(ChartFilter chartFilter);

        Dictionary<int, Point> GetPriceChartPoints(ChartFilter chartFilter);

        Dictionary<int, Point> GetMarketChartPoints(ChartFilter chartFilter);

        IEnumerable<League> GetLeagues(GetLeaguesCriteria getLeaguesCriteria);

        IEnumerable<Match> GetMatches(GetMatchesCriteria getMatchesCriteria);

        IEnumerable<Odds> GetOdds(GetOddsCriteria getOddsCriteria);

        IEnumerable<OddsGroup> GetOddsGroup(GetOddsCriteria getOddsCriteria);

        Dictionary<int, Point> GetEventChartPoints(ChartFilter chartFilter);

        IEnumerable<IndicatorPoint> GetIndicatorPoints(GetIndicatorPointCriteria getIndicatorPointCriteria);

        Match GetMatchDetail(GetMatchDetailCriteria getMatchDetailCriteria);

        bool CurrentMatchIsLive(int matchId, DateTime kickOffTime, ref float openSoonCountdown);

        CurrentMatchTime GetCurrentMatchTime(GetCurrentMatchTimeCriteria criteria);
    }
}