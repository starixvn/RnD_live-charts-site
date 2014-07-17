// --------------------------------------------------------------------------
// <copyright file="AuthenticationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.4">Created</change>
// </history>

namespace Fanex.LiveCharts.Authentication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;
    using Fanex.LiveCharts.Authentication.Core;
    using Fanex.LiveCharts.Authentication.Criterias;
    using Fanex.LiveCharts.Authentication.Models;
    using Fanex.LiveCharts.ClientAuthentication.Models;

    /// <summary>
    /// Authentication Service.
    /// </summary>
    public class AuthenticationService : ServiceBase
    {
        private const string RandomString = "1q2w3e4r!@#$";
        private const string CacheString = "Fanex.LiveCharts.Authentication";
        private const string CacheGetCharts = "Fanex.LiveCharts.CacheGetCharts";
        private const int GMTMinus4 = -4;

        /// <summary>
        /// Get Token for viewing chart.
        /// </summary>
        /// <param name="memberId">Member id need view chart.</param>
        /// <param name="encryptText">Encrypt text for check valid data.</param>
        /// <param name="matchId">Current match id for check encrypt text.</param>
        /// <returns>Token string.</returns>
        public string GetToken(int memberId, string encryptText, int matchId)
        {
            try
            {
                //// Check valid Ecrypt Text or not.
                if (Cryptography.CfsDeCrypt(encryptText) != (RandomString + memberId.ToString() + matchId.ToString()))
                {
                    throw new Exception(string.Format("Protect string is not valid with memberid {0}", memberId));
                }

                var currentToken = Guid.NewGuid().ToString();
                var tokenCacheKey = string.Format(
                    "{0}_{1}_{2}",
                    CacheString,
                    memberId,
                    matchId);
                CouchBaseCachingService.Set<string>(
                    tokenCacheKey,
                    currentToken,
                    new TimeSpan(0, AppSettings.CachingTimeOutMinute, 0));

                return currentToken;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// Get list chart information.
        /// </summary>
        /// <param name="kickOffDate">Kick off date.</param>
        /// <param name="sportId">Get chart from sport _type.</param>
        /// <param name="timeZone">Current time zone to get chart.</param>
        /// <param name="matchStatus">Chart is running or completed.</param>
        /// <param name="chartShowingStatus">Chart status for showing on production.</param>
        /// <param name="numOfEarlyDay">Early number for get early matches.</param>
        /// <returns>List chart information.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public ChartInfo[] GetCharts(
            DateTime kickOffDate,
            int sportId = 0,
            int timeZone = -4,
            string matchStatus = "",
            byte chartShowingStatus = 1,
            byte numOfEarlyDay = 0)
        {
            try
            {
                kickOffDate = kickOffDate.AddHours(timeZone - GMTMinus4);
                var getCachedChartsKey = string.Format(
                    "{0}_{1}_{2}_{3}_{4}_{5}_{6}",
                    CacheGetCharts,
                    kickOffDate.ToShortDateString(),
                    sportId,
                    timeZone,
                    matchStatus,
                    chartShowingStatus,
                    numOfEarlyDay);
                ChartInfo[] charts = CouchBaseCachingService.Get<ChartInfo[]>(getCachedChartsKey);
                if (charts == null)
                {
                    IEnumerable<ChartFlat> chartFlats = Fetch<ChartFlat>(new GetChartsCriteria
                        {
                            MatchStatus = matchStatus.ToLower(),
                            KickOffDate = kickOffDate,
                            SportId = sportId,
                            ChartShowingStatus = chartShowingStatus,
                            NumOfEarlyDay = numOfEarlyDay
                        });
                    charts = chartFlats.GroupBy(c => new { c.MatchId, c.MatchStatus, c.KickOffTime })
                        .Select(g =>
                        {
                            var chart = new ChartInfo();
                            chart.MatchId = g.Key.MatchId;
                            chart.MatchStatus = g.Key.MatchStatus;
                            chart.KickOffTime = g.Key.KickOffTime;
                            chart.BetTypeList = g.GroupBy(cGroup => cGroup.BetTypeId)
                                                .Select(bt =>
                                                {
                                                    var betType = new BetTypeInfo();
                                                    betType.BetTypeId = bt.Key;
                                                    betType.OddsIds = bt.Select(odds => odds.OddsId).ToArray();
                                                    return betType;
                                                }).OrderBy(bt => bt.BetTypeId).ToArray();
                            return chart;
                        }).ToArray();
                    CouchBaseCachingService.Set<ChartInfo[]>(getCachedChartsKey, charts, new TimeSpan(0, 0, AppSettings.CachingGetChartsTimeOutSeconds));
                }

                return charts;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return new ChartInfo[0];
        }
    }
}