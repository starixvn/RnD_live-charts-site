// --------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Resources;

    /// <summary>
    /// Constants information.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
    public class Constants
    {
        public const int MuteWholeMatchTimeMinute = 100;
        public const int LIVE_MATCH_INTERVAL = 1;
        public const int DEAD_BALL_MATCH_INTERVAL = 15;
        public const int LIVE_START_MINUTE = -1;
        public const int LIVE_STOP_MINUTE = 120;
        public const int DEAD_BALL_START_MINUTE = -1455;
        public const int DEAD_BALL_STOP_MINUTE = 0;
        public const int REMOVE_CACHE_BEFORE_MINUTE = 4;
        public const int LIVE_TIME_OVER = LIVE_STOP_MINUTE + LIVE_MATCH_INTERVAL;
        public const int DEAD_BALL_TIME_OVER = DEAD_BALL_STOP_MINUTE + DEAD_BALL_MATCH_INTERVAL;
        public const int MIN_STEP_SECONDS = 10;
        public const string FORMAT_DATE_TIME = "dd MMM, yyyy hh:mm tt";
        public const string FORMAT_DATE = "dd MMM, yyyy";
        public static TimeSpan SPAN_CACHING_TIME_FOR_LIVE_MATCH = new TimeSpan(0, 3, 0, 0);
        public static TimeSpan SPAN_CACHING_TIME_FOR_DEAD_BALL_MATCH = new TimeSpan(1, 1, 0, 0);
        public static LockProvider<string> LockGetPoints = new LockProvider<string>();
        public static LockProvider<string> LockGetCache = new LockProvider<string>();

        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        static Constants()
        {
            BetTypes = new List<BetType>
            {
                new BetType
                {
                    Id = (int)BetTypeEnum.Handicap,
                    Name = Lang.Handicap
                },
                new BetType
                {
                    Id = (int)BetTypeEnum.OverUnder,
                    Name = Lang.OverUnder
                },
                new BetType
                {
                    Id = (int)BetTypeEnum._1X2,
                    Name = Lang._1x2
                }
            };

            OddsChoices = new List<TeamChoice>
            {
                new TeamChoice
                {
                    BetTypeId = (int)BetTypeEnum.Handicap,
                    Id = (int)HandicapChoice.Favorite,
                    Name = Lang.Favorite
                },
                new TeamChoice
                {
                    BetTypeId = (int)BetTypeEnum.Handicap,
                    Id = (int)HandicapChoice.Underdog,
                    Name = Lang.Underdog
                },
                new TeamChoice
                {
                    BetTypeId = (int)BetTypeEnum.OverUnder,
                    Id = (int)OverUnderChoice.Over,
                    Name = Lang.Over
                },
                new TeamChoice
                {
                    BetTypeId = (int)BetTypeEnum.OverUnder,
                    Id = (int)OverUnderChoice.Under,
                    Name = Lang.Under
                },
                new TeamChoice
                {
                    BetTypeId = (int)BetTypeEnum._1X2,
                    Id = (int)_1x2Choice._1,
                    Name = Lang._1
                },
                new TeamChoice
                {
                    BetTypeId = (int)BetTypeEnum._1X2,
                    Id = (int)_1x2Choice._x,
                    Name = Lang._x
                },
                new TeamChoice
                {
                    BetTypeId = (int)BetTypeEnum._1X2,
                    Id = (int)_1x2Choice._2,
                    Name = Lang._2
                }
            };
        }

        public static IList<BetType> BetTypes { get; set; }

        public static IList<TeamChoice> OddsChoices { get; set; }
    }
}