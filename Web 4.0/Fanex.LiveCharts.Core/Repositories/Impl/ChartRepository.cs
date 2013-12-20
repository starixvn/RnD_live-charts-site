// --------------------------------------------------------------------------
// <copyright file="ChartRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
//   <change who="Jason.Ly" date="2013.7.24">Add new methods</change>
// </history>

namespace Fanex.LiveCharts.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Repositories.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Chart repository, handle all query data.
    /// </summary>
    public class ChartRepository : BaseRepository, IChartRepository
    {
        /// <summary>
        /// Get current job time.
        /// </summary>
        /// <param name="criteria">Get job time criteria.</param>
        /// <returns>Current job time.</returns>
        public DateTime? GetMaxTime(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.GetSpParams();
                    objDb.ExecuteNonQuery(parameters);

                    var maxTime = Converter.ToDateTime(parameters["maxtime"]);
                    return maxTime;
                }
            }

            return null;
        }

        /// <summary>
        /// Fetch data from database.
        /// </summary>
        /// <typeparam name="T">Object type return.</typeparam>
        /// <param name="criteria">Get data criteria.</param>
        /// <param name="maxTime">Current max time.</param>
        /// <returns>List of object.</returns>
        public IEnumerable<T> Fetch<T>(ICriteria criteria, ref DateTime maxTime)
        {
            if (criteria.IsValid())
            {
                using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.GetSpParams();
                    var list = objDb.Query<T>(parameters);

                    maxTime = Converter.ToDateTime(parameters["maxtime"]);
                    return list;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Current Match time.
        /// </summary>
        /// <param name="criteria">Get Current Match time criteria.</param>
        /// <returns>Current match time in minute.</returns>
        public CurrentMatchTime GetCurrentMatchTime(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.GetSpParams();
                    return objDb.QueryEntity<CurrentMatchTime>(parameters);
                }
            }

            return new CurrentMatchTime();
        }

        /// <summary>
        /// Get Enable Chart with Odds information.
        /// </summary>
        /// <param name="criteria">Get Enable chart criteria.</param>
        /// <returns>Match list.</returns>
        public IEnumerable<Match> GetEnableCharts(ICriteria criteria)
        {
            List<Match> matches = new List<Match>();
            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        Match currentMatch = null;
                        var matchId = 0;
                        while (reader.Read())
                        {
                            int currentMatchId = reader.ToInt("matchid");
                            if (currentMatchId != matchId)
                            {
                                currentMatch = new Match();
                                matches.Add(currentMatch);
                                currentMatch.Id = currentMatchId;
                                currentMatch.HomeId = reader.ToInt("homeid");
                                currentMatch.AwayId = reader.ToInt("awayid");
                                currentMatch.KickOffTime = reader.ToDateTime("kickofftime");
                                currentMatch.ChartShowingStatus = reader.To<byte>("chartshowingstatus");
                                matchId = currentMatchId;
                            }

                            var currentOddsId = reader.ToInt("oddsid");
                            var betTypeId = reader.ToShort("bettype");
                            if (currentMatch != null && currentOddsId > 0 && betTypeId != (int)BetTypeEnum._1X2)
                            {
                                currentMatch.OddsList.Add(new Odds
                                {
                                    Id = currentOddsId,
                                    BetTypeId = reader.ToShort("bettype"),
                                    IsLive = reader.ToBool("liveindicator"),
                                    HomeHandicap = reader.ToDouble("hdp1"),
                                    AwayHandicap = reader.ToDouble("hdp2"),
                                    ChartStatus = reader.ToBool("chartstatus")
                                });
                            }
                        }

                        return matches;
                    }
                }
            }

            return matches;
        }
    }
}