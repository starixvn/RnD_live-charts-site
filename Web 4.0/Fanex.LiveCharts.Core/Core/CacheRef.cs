// --------------------------------------------------------------------------
// <copyright file="CacheRef.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Core
{
    using System.Threading;
    using RemotingRef;

    /// <summary>
    /// Remote reference class.
    /// </summary>
    public static class CacheRef
    {
        public static string CurrentLanguage
        {
            get
            {
                switch (Thread.CurrentThread.CurrentCulture.Name)
                {
                    case "en-US": return "en";
                    case "zh-TW": return "ch"; // Traditional
                    case "zh-CN": return "cs"; // Simplified
                    case "ja-JP": return "jp";
                    case "th-TH": return "th"; // Thai
                    case "ko-KR": return "ko"; // Korean
                    case "vi-VN": return "vn"; // Vietnamese
                    default: return "en";
                }
            }
        }
        
        /// <summary>
        /// Get bet type name from bet type id.
        /// </summary>
        /// <param name="bettypeId">Bet type Id.</param>
        /// <returns>Bet type name.</returns>
        public static string GetBettype(object bettypeId)
        {
            return GetBettype(bettypeId, null);
        }

        /// <summary>
        /// Get Bet type from bet type id and bet id.
        /// </summary>
        /// <param name="bettypeId">Bet type id.</param>
        /// <param name="betID">Betting id.</param>
        /// <returns>Bet type name.</returns>
        public static string GetBettype(object bettypeId, object betID)
        {
            // resourceId remove first character
            string resourceId = string.Empty;
            if (betID != null && betID.ToString().Length > 1)
            {
                resourceId = betID.ToString().Substring(1);
            }

            return RefData.GetBetTypeName(bettypeId.ToString(), CurrentLanguage, string.Empty, resourceId);
        }

        /// <summary>
        /// Get sport name from sport id.
        /// </summary>
        /// <param name="sportId">Sport id for get name.</param>
        /// <returns>Sport name string.</returns>
        public static string GetSportName(object sportId)
        {
            return RefData.GetSportName(sportId.ToString(), CurrentLanguage, string.Empty);
        }

        /// <summary>
        /// Get team name from team id.
        /// </summary>
        /// <param name="teamId">Team id for get team name.</param>
        /// <returns>Team name.</returns>
        public static string GetTeamName(object teamId)
        {
            return RefData.GetTeamName(teamId.ToString(), CurrentLanguage, string.Empty);
        }
        
        /// <summary>
        /// Get league name from league id.
        /// </summary>
        /// <param name="leagueId">League id for get name.</param>
        /// <returns>League name.</returns>
        public static string GetLeagueName(object leagueId)
        {
            return RefData.GetLeagueName(leagueId.ToString(), CurrentLanguage, string.Empty);
        }

        /// <summary>
        /// Get Odds name from odds type id.
        /// </summary>
        /// <param name="oddsTypeId">Odds type id.</param>
        /// <returns>Odds type name.</returns>
        public static string GetOddsTypeName(object oddsTypeId)
        {
            return RefData.GetOddsTypeName(oddsTypeId.ToString(), CurrentLanguage);
        }
    }
}