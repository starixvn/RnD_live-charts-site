// --------------------------------------------------------------------------
// <copyright file="ChartService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Common;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Repositories.Impl;
    using Fanex.LiveCharts.Repositories.Interface;
    using Fanex.LiveCharts.Resources;
    using Fanex.LiveCharts.Services.Interface;

    /// <summary>
    /// Chart Service handles everything about chart.
    /// </summary>
    public class ChartService : ServiceBase, IChartService
    {
        private readonly IChartRepository _chartRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartService" /> class.
        /// </summary>
        public ChartService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartService" /> class.
        /// </summary>
        /// <param name="movementChartRepository">Movement chart repository.</param>
        public ChartService(IChartRepository movementChartRepository)
        {
            _chartRepository = movementChartRepository ?? new ChartRepository();
        }

        /// <summary>
        /// Get Leagues from database with ordered name.
        /// </summary>
        /// <param name="getLeaguesCriteria">Criteria for get league.</param>
        /// <returns>League return.</returns>
        public IEnumerable<League> GetLeagues(GetLeaguesCriteria getLeaguesCriteria)
        {
            try
            {
                var result = _chartRepository.Fetch<League>(getLeaguesCriteria).OrderBy(l => l.Name);
                return result;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new List<League>();
            }
        }

        /// <summary>
        /// Get Match and order by match name.
        /// </summary>
        /// <param name="getMatchesCriteria">Criteria for get match.</param>
        /// <returns>List matches.</returns>
        public IEnumerable<Match> GetMatches(GetMatchesCriteria getMatchesCriteria)
        {
            try
            {
                var result = _chartRepository.Fetch<Match>(getMatchesCriteria).OrderBy(m => m.MatchName);
                return result;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new List<Match>();
            }
        }

        /// <summary>
        /// Get two main odds.
        /// </summary>
        /// <param name="getOddsCriteria">Get odds criteria.</param>
        /// <returns>Odds list.</returns>
        public IEnumerable<Odds> GetOdds(GetOddsCriteria getOddsCriteria)
        {
            try
            {
                var result = _chartRepository.Fetch<Odds>(getOddsCriteria);
                return result ?? new List<Odds>();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new List<Odds>();
            }
        }

        /// <summary>
        /// Get Odds group to update odd in Admin site.
        /// </summary>
        /// <param name="getOddsCriteria">Get Odds criteria.</param>
        /// <returns>Odds group list with bet type.</returns>
        public IEnumerable<OddsGroup> GetOddsGroup(GetOddsCriteria getOddsCriteria)
        {
            try
            {
                var oddsList = GetOdds(getOddsCriteria);
                var oddsGroups = oddsList.GroupBy(o => o.BetTypeId).Select(og =>
                    {
                        return new OddsGroup
                        {
                            BetTypeId = og.Key,
                            OddsList = og.ToList()
                        };
                    }).OrderBy(ogg => ogg.BetTypeId);

                return oddsGroups;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new List<OddsGroup>();
            }
        }

        /// <summary>
        /// Get match detail.
        /// </summary>
        /// <param name="getMatchDetailCriteria">Get match criteria.</param>
        /// <returns>Match details.</returns>
        public Match GetMatchDetail(GetMatchDetailCriteria getMatchDetailCriteria)
        {
            try
            {
                return _chartRepository.Get<Match>(getMatchDetailCriteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new Match();
            }
        }

        /// <summary>
        /// Get Current Match time.
        /// </summary>
        /// <param name="criteria">Get Current Match time criteria.</param>
        /// <returns>Current match time in minute.</returns>
        public CurrentMatchTime GetCurrentMatchTime(GetCurrentMatchTimeCriteria criteria)
        {
            try
            {
                return _chartRepository.GetCurrentMatchTime(criteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new CurrentMatchTime();
            }
        }

        /// <summary>
        /// First Initial Chart, Get Data from Database, if have data, then update that data to cache.
        /// </summary>
        /// <param name="chartFilter">Chart filter.</param>
        /// <returns>List chart needed.</returns>
        public IEnumerable<Chart> GetCharts(ChartFilter chartFilter)
        {
            var charts = new List<Chart>();
            try
            {
                if (!string.IsNullOrEmpty(chartFilter.OddsIds))
                {
                    var oddsIds = chartFilter.OddsIds.Split(',');
                    if (oddsIds.Length > 0)
                    {
                        charts.Add(InitChart(chartFilter, ChartType.Market, Converter.ToInt(oddsIds[0])));
                        charts.Add(InitChart(chartFilter, ChartType.Event, Converter.ToInt(oddsIds[0])));
                        foreach (var oddsId in oddsIds)
                        {
                            charts.Add(InitChart(chartFilter, ChartType.Price, Converter.ToInt(oddsId)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return charts;
        }

        /// <summary>
        /// Get single Chart.
        /// </summary>
        /// <param name="chartFilter">Chart filter for get data.</param>
        /// <returns>Chart information.</returns>
        public Chart GetChart(ChartFilter chartFilter)
        {
            var chart = new Chart();
            try
            {
                chart = InitChart(chartFilter, (ChartType)chartFilter.ChartId, chartFilter.OddsId);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return chart;
        }

        /// <summary>
        /// Get Points Of Price Chart.
        /// </summary>
        /// <param name="chartFilter">Chart filter.</param>
        /// <returns>Points with filtered.</returns>
        public Dictionary<int, Point> GetPriceChartPoints(ChartFilter chartFilter)
        {
            chartFilter.ChartId = (int)ChartType.Price;
            return GetPoints(chartFilter);
        }

        /// <summary>
        /// Get Point Of Market Chart.
        /// </summary>
        /// <param name="chartFilter">Chart filter.</param>
        /// <returns>Points with filtered.</returns>
        public Dictionary<int, Point> GetMarketChartPoints(ChartFilter chartFilter)
        {
            chartFilter.ChartId = (int)ChartType.Market;
            return GetPoints(chartFilter);
        }

        /// <summary>
        /// Get indicator points.
        /// </summary>
        /// <param name="getIndicatorPointCriteria">Get indicator point criteria.</param>
        /// <returns>Indicator point list.</returns>
        public IEnumerable<IndicatorPoint> GetIndicatorPoints(GetIndicatorPointCriteria getIndicatorPointCriteria)
        {
            try
            {
                var indicatorPoints = _chartRepository.Fetch<IndicatorPoint>(getIndicatorPointCriteria);
                return indicatorPoints;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new List<IndicatorPoint>();
            }
        }

        /// <summary>
        /// Chart filter for get event points.
        /// </summary>
        /// <param name="chartFilter">Filter for get information.</param>
        /// <returns>Event Point list.</returns>
        public Dictionary<int, Point> GetEventChartPoints(ChartFilter chartFilter)
        {
            chartFilter.ChartId = (int)ChartType.Event;
            return GetPoints(chartFilter);
        }

        public bool IsMatchCompletedCapture(IsMatchCompleteCaptureCriteria criteria)
        {
            return _chartRepository.Count(criteria) > 0;
        }

        /// <summary>
        /// Check current match id live or not.
        /// </summary>
        /// <param name="matchId">Match id for check match started or not.</param>
        /// <param name="kickOffTime">Match kick off time.</param>
        /// <param name="openSoonCountdown">Open soon milliseconds countdown.</param>
        /// <returns>Live or not.</returns>
        public bool CurrentMatchIsLive(int matchId, DateTime kickOffTime, ref float openSoonCountdown)
        {
            try
            {
                var maxTime = _chartRepository.GetMaxTime(new GetMaxTimeCriteria
                {
                    ChartType = (int)ChartType.Market
                });

                if (maxTime == null)
                {
                    return true;
                }

                TimeSpan timeSpan = (TimeSpan)(maxTime - kickOffTime);
                int beforeSeconds = 20;
                int beforeMinuteForSureMatchStarted = 15;
                if (timeSpan.TotalSeconds >= beforeSeconds)
                {
                    if (timeSpan.TotalMinutes > beforeMinuteForSureMatchStarted)
                    {
                        return true;
                    }

                    var currentMatchTime = GetCurrentMatchTime(new GetCurrentMatchTimeCriteria
                    {
                        MatchId = matchId
                    });

                    if (currentMatchTime.LivePeriod > 0)
                    {
                        return true;
                    }
                    //// If max time greater thang kick off time but match still have not kicked off, then wait more 60 seconds
                    openSoonCountdown = 60000;
                    return false;
                }
                else
                {
                    openSoonCountdown = Math.Abs((float)timeSpan.TotalMilliseconds) + (beforeSeconds * 1000);
                }

                return false;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return true;
            }
        }

        #region Private Methods

        /// <summary>
        /// Get Points.
        /// </summary>
        /// <param name="chartFilter">Char filter.</param>
        /// <returns>Points list with filtered.</returns>
        private Dictionary<int, Point> GetPoints(ChartFilter chartFilter)
        {
            Dictionary<int, Point> points = new Dictionary<int, Point>();

            try
            {
                Dictionary<int, Point> pointsForUpdateCache = null;

                //// Get data from cache
                var pointsFromCache = this.GetPointsFromCache(chartFilter);

                //// IF there is no data from cache, then get from db
                if (!pointsFromCache.Any())
                {
                    //// Get Point From Database
                    points = this.GetPointsFromDatabase(chartFilter);
                    if (points != null && points.Any())
                    {
                        pointsForUpdateCache = points;
                    }
                    else
                    {
                        return points;
                    }
                }
                else
                {
                    points = pointsFromCache;
                }

                //// Update Cache if have New data From DB
                if (pointsForUpdateCache != null && pointsForUpdateCache.Any())
                {
                    this.UpdateCache(chartFilter, pointsForUpdateCache);
                }

                if (points.Count <= 1)
                {
                    return points;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return points.OrderBy(pair => pair.Key).ToDictionary(key => key.Key, value => value.Value);
        }

        /// <summary>
        /// Initial chart information.
        /// </summary>
        /// <param name="chartFilter">Get chart filter.</param>
        /// <param name="chartType">Current chart type.</param>
        /// <param name="oddsId">Odds id for get data.</param>
        /// <returns>Chart information.</returns>
        private Chart InitChart(ChartFilter chartFilter, ChartType chartType, int oddsId = 0)
        {
            if (oddsId == 0)
            {
                return new Chart();
            }

            var chart = new Chart
            {
                Name = chartType == ChartType.Price ? Lang.PriceMovement : Lang.MarketMovement,
                Id = (int)chartType,
                BetTypeId = chartFilter.BetTypeId,
                OddsId = oddsId,
                CurrentMatch = new Match
                {
                    Id = chartFilter.MatchId,
                    IsLive = chartFilter.IsLive,
                    KickOffTime = chartFilter.KickOffTime
                }
            };

            //// Get Point from DB
            var jobTime = DateTime.Now;
            var criteria = new GetPointsCriteria
            {
                BetType = chartFilter.BetTypeId,
                ChartId = (int)chartType,
                OddsId = oddsId,
                MatchId = chartFilter.MatchId,
                Minute = chartFilter.DefaultStartTime
            };

            var points = _chartRepository.Fetch<Point>(criteria, ref jobTime);

            //// Get Current Maxtime of Job
            var maxTime = GetMaxTime(chart.Points, chartFilter, jobTime, chartFilter.DefaultStartTime);

            //// Update data for Dictionary
            this.UpdateChartPoints(chartType, chart.Points, points, maxTime);

            chart.Points = AddDataForMissingPoint(chartFilter.Clone(chartType), chart.Points, true);

            if ((chartFilter.IsLive && maxTime <= Constants.LIVE_STOP_MINUTE)
                || (!chartFilter.IsLive && maxTime < Constants.DEAD_BALL_STOP_MINUTE))
            {
                UpdateCache(chart, maxTime < Constants.REMOVE_CACHE_BEFORE_MINUTE);
            }

            return chart;
        }

        /// <summary>
        /// Update data to dictionary.
        /// </summary>
        /// <param name="chartType">Current chart type.</param>
        /// <param name="pointDic">Dictionary contains all points.</param>
        /// <param name="points">Points need to be updated to dictionary.</param>
        /// <param name="maxTime">Current max time to add empty point.</param>
        private void UpdateChartPoints(ChartType chartType, Dictionary<int, Point> pointDic, IEnumerable<Point> points, int maxTime)
        {
            foreach (var point in points)
            {
                if (point.TimeIndex <= maxTime && pointDic.ContainsKey(point.TimeIndex))
                {
                    if (chartType == ChartType.Market && point.TotalTickets != 0)
                    {
                        pointDic[point.TimeIndex].HomeTickets += point.HomeTickets;
                        pointDic[point.TimeIndex].AwayTickets += point.AwayTickets;
                    }
                    else if (chartType == ChartType.Event
                        && pointDic[point.TimeIndex].EventType > 0
                        && point.EventType > 0)
                    {
                        pointDic[point.TimeIndex].SubPoints.Add(point);
                    }
                    else
                    {
                        pointDic[point.TimeIndex] = point;
                    }
                }
            }
        }

        /// <summary>
        /// Get Max time of job, and create no data point.
        /// </summary>
        /// <param name="points">Point list.</param>
        /// <param name="chartFilter">Current chart filter.</param>
        /// <param name="jobTime">Current job time.</param>
        /// <param name="fromMinute">From minute.</param>
        /// <returns>Current max time.</returns>
        private int GetMaxTime(Dictionary<int, Point> points, ChartFilter chartFilter, DateTime jobTime, int fromMinute)
        {
            var addSeconds = chartFilter.ChartId == (int)ChartType.Price ? 0 : 30;
            int toMinute = (int)(jobTime - chartFilter.KickOffTime.AddSeconds(addSeconds)).TotalMinutes;

            ////#region For Testing, will Remove

            ////if (!chartFilter.IsLive)
            ////{
            ////    toMinute = ++toMinute * Constants.DEAD_BALL_MATCH_INTERVAL + Constants.DEAD_BALL_START_MINUTE;
            ////}

            ////#endregion For Testing, will Remove

            //// IF toMinute exceed constrain, then set it equals to max value1.
            if (chartFilter.IsLive && toMinute > Constants.LIVE_TIME_OVER)
            {
                toMinute = Constants.LIVE_TIME_OVER;
            }
            else if (!chartFilter.IsLive && toMinute > Constants.DEAD_BALL_TIME_OVER)
            {
                toMinute = Constants.DEAD_BALL_TIME_OVER;
            }

            //// If have point, then add zero data
            if (points != null)
            {
                var step = chartFilter.ChartInterval;
                for (int i = fromMinute + step; i <= toMinute; i += step)
                {
                    points[i] = new Point
                    {
                        TimeIndex = (short)i,
                        RecordTime = chartFilter.KickOffTime.AddMinutes(i),
                        HomeTickets = 0,
                        AwayTickets = 0,
                        DrawTickets = 0,
                        BetTypeId = (short)chartFilter.BetTypeId
                    };
                }
            }

            return toMinute;
        }

        /// <summary>
        /// Get points from database.
        /// </summary>
        /// <param name="chartFilter">Get Point criteria.</param>
        /// <returns>Point list.</returns>
        private Dictionary<int, Point> GetPointsFromDatabase(ChartFilter chartFilter)
        {
            var pointsDic = new Dictionary<int, Point>();
            IEnumerable<Point> points = new List<Point>();
            ////_movementChartRepository.Fetch<MovingPointDetail>(getMovingPointCriteria);
            var lockString = GetChartKey(chartFilter);
            if (Monitor.TryEnter(Constants.LockGetPoints.GetLock(lockString)))
            {
                try
                {
                    var jobTime = DateTime.Now;
                    var getPointCriteria = new GetPointsCriteria
                    {
                        BetType = chartFilter.BetTypeId,
                        OddsId = chartFilter.OddsId,
                        Minute = chartFilter.CurrentTimeIndex,
                        MatchId = chartFilter.MatchId,
                        ChartId = chartFilter.ChartId
                    };

                    points = _chartRepository.Fetch<Point>(getPointCriteria, ref jobTime);

                    //// Get Max Time of job
                    var maxTime = GetMaxTime(pointsDic, chartFilter, jobTime, chartFilter.CurrentTimeIndex);
                    this.UpdateChartPoints((ChartType)chartFilter.ChartId, pointsDic, points, maxTime);
                    pointsDic = AddDataForMissingPoint(chartFilter, pointsDic, false);
                }
                catch (Exception ex)
                {
                    ExceptionHandler(ex);
                }
                finally
                {
                    Monitor.Exit(Constants.LockGetPoints.GetLock(lockString));
                    ////Constants.LockGetPoints.RemoveLock(lockString);
                }
            }

            return pointsDic;
        }

        /// <summary>
        /// Get cache key for get chart data.
        /// </summary>
        /// <param name="chartFilter">Chart filter.</param>
        /// <returns>Cache key.</returns>
        private string GetChartKey(ChartFilter chartFilter)
        {
            return string.Format("{0}_{1}_{2}_{3}", chartFilter.ChartId, chartFilter.MatchId, chartFilter.OddsId, chartFilter.BetTypeId);
        }

        /// <summary>
        /// Get Point from Cache.
        /// </summary>
        /// <param name="chartFilter">Chart filter information.</param>
        /// <returns>Points from cache.</returns>
        private Dictionary<int, Point> GetPointsFromCache(ChartFilter chartFilter)
        {
            var cacheKey = GetChartKey(chartFilter);

            Chart chart = null;
            lock (Constants.LockGetCache.GetLock(cacheKey))
            {
                chart = CacheManager.Instance.Get<Chart>(cacheKey);
            }

            var cachePoints = new Dictionary<int, Point>();

            //// If have data from Cache, then get specific range
            if (chart != null && chart.Points.Keys.Any())
            {
                var toTime = chart.Points.Keys.Max();
                var step = chartFilter.ChartInterval;
                for (int i = chartFilter.CurrentTimeIndex + step; i <= toTime; i += step)
                {
                    if (chart.Points.Keys.Contains(i))
                    {
                        cachePoints[i] = chart.Points[i];
                    }
                }
            }

            return cachePoints;
        }

        /// <summary>
        /// Get Last point from cache, for copy Odd data for missing Point.
        /// </summary>
        /// <param name="chartFilter">Chart filter information.</param>
        /// <returns>Last point from cache.</returns>
        private Point GetLastPointFromCache(ChartFilter chartFilter)
        {
            var cacheKey = GetChartKey(chartFilter);

            Chart chart = null;
            lock (Constants.LockGetCache.GetLock(cacheKey))
            {
                chart = CacheManager.Instance.Get<Chart>(cacheKey);
            }

            var cachePoints = new Dictionary<int, Point>();
            if (chart != null && chart.Points.Any())
            {
                return chart.Points.Last().Value;
            }

            return null;
        }

        /// <summary>
        /// Update chart Data to cache, if no chart then insert
        /// if has chart then update data.
        /// </summary>
        /// <param name="chartObject">Chart data for update to cache.</param>
        /// <param name="removeOld">Remove point in cache.</param>
        private void UpdateCache(Chart chartObject, bool removeOld = false)
        {
            Task.Factory.StartNew(
                (chartObj) =>
                {
                    try
                    {
                        var chart = (Chart)chartObj;
                        if (chart.Points.Any() || removeOld)
                        {
                            var cacheKey = string.Format("{0}_{1}_{2}_{3}", chart.Id, chart.CurrentMatch.Id, chart.OddsId, chart.BetTypeId);
                            Chart cachedChart = null;
                            lock (Constants.LockGetCache.GetLock(cacheKey))
                            {
                                cachedChart = CacheManager.Instance.Get<Chart>(cacheKey);
                                if (cachedChart == null)
                                {
                                    //// If current Chart is null then Cache Data for chart
                                    cachedChart = chart;
                                }
                                else
                                {
                                    if (removeOld)
                                    {
                                        cachedChart.Points = new Dictionary<int, Point>();
                                    }
                                    //// If Current chart is in cache, then update
                                    foreach (var point in chart.Points)
                                    {
                                        cachedChart.Points[point.Key] = point.Value;
                                    }
                                }

                                CacheManager.Instance.Set<Chart>(cacheKey, cachedChart, new TimeSpan(0, 10, 0));
#if DEBUG
                                ////Debug.WriteLine(string.Format("Update Cache: {0} - {1}", cacheKey, cachedChart.Points.Count));
#endif
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler(ex);
                    }
                },
                (object)chartObject);
        }

        /// <summary>
        /// Over Load Update cache.
        /// </summary>
        /// <param name="chartFilter">Chart filter information.</param>
        /// <param name="newPoints">New points for update cache.</param>
        private void UpdateCache(ChartFilter chartFilter, Dictionary<int, Point> newPoints)
        {
            //// Use other thread is here
            this.UpdateCache(new Chart
            {
                Id = chartFilter.ChartId,
                CurrentMatch = new Match
                {
                    Id = chartFilter.MatchId,
                    IsLive = chartFilter.IsLive,
                    KickOffTime = chartFilter.KickOffTime,
                },
                OddsId = chartFilter.OddsId,
                BetTypeId = chartFilter.BetTypeId,
                Points = newPoints
            });
        }

        /// <summary>
        /// Add data for missing point with no ticket.
        /// </summary>
        /// <param name="chartFilter">Chart filter for get cache object.</param>
        /// <param name="points">Point need add missing point.</param>
        /// <param name="initChart">Is initial chart or not.</param>
        /// <returns>Point dictionary.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private Dictionary<int, Point> AddDataForMissingPoint(ChartFilter chartFilter, Dictionary<int, Point> points, bool initChart)
        {
            if (chartFilter.ChartId == (int)ChartType.Price)
            {
                var homeOdds = 0d;
                var awayOdds = 0d;
                var drawOdds = 0d;
                var homeHdp = 0d;
                var awayHdp = 0d;
                var timeIndex = chartFilter.IsLive ? 0 : Constants.DEAD_BALL_START_MINUTE + Constants.DEAD_BALL_MATCH_INTERVAL;
                var stopTime = chartFilter.DefaultStopTime;
                if (!initChart)
                {
                    Point lastPoint = GetLastPointFromCache(chartFilter);
                    if (lastPoint != null && lastPoint.TimeIndex < stopTime)
                    {
                        homeOdds = lastPoint.HomeOdds;
                        awayOdds = lastPoint.AwayOdds;
                        drawOdds = lastPoint.DrawOdds;
                        timeIndex = lastPoint.TimeIndex;
                        homeHdp = lastPoint.HomeHandicap;
                        awayHdp = lastPoint.AwayHandicap;
                    }
                }

                var isChangeHdp = false;

                foreach (var point in points)
                {
                    isChangeHdp = false;
                    points[point.Key].HomeOdds = Math.Round(points[point.Key].HomeOdds, 2);
                    points[point.Key].DrawOdds = Math.Round(points[point.Key].DrawOdds, 2);
                    points[point.Key].AwayOdds = Math.Round(points[point.Key].AwayOdds, 2);

                    point.Value.BetTypeId = (short)chartFilter.BetTypeId;
                    if (point.Value.HomeOdds == 0)
                    {
                        points[point.Key].HomeOdds = homeOdds;
                        isChangeHdp = true;
                    }
                    else
                    {
                        homeOdds = points[point.Key].HomeOdds;
                    }

                    if (point.Value.DrawOdds == 0)
                    {
                        points[point.Key].DrawOdds = drawOdds;
                    }
                    else
                    {
                        drawOdds = points[point.Key].DrawOdds;
                    }

                    if (point.Value.AwayOdds == 0)
                    {
                        isChangeHdp = true;
                        points[point.Key].AwayOdds = awayOdds;
                    }
                    else
                    {
                        awayOdds = points[point.Key].AwayOdds;
                    }

                    if (isChangeHdp)
                    {
                        if (point.Value.HomeHandicap == 0)
                        {
                            points[point.Key].HomeHandicap = homeHdp;
                        }

                        if (point.Value.AwayHandicap == 0)
                        {
                            points[point.Key].AwayHandicap = awayHdp;
                        }
                    }

                    homeHdp = points[point.Key].HomeHandicap;
                    awayHdp = points[point.Key].AwayHandicap;
                }
            }

            return points;
        }

        #endregion Private Methods
    }
}