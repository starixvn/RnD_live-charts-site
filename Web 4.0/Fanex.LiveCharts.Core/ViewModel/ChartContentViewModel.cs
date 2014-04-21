// --------------------------------------------------------------------------
// <copyright file="ChartContentViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.16">Created</change>
// </history>

namespace Fanex.LiveCharts.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Models;

    /// <summary>
    /// Chart Content View Model.
    /// </summary>
    public class ChartContentViewModel
    {
        public int BetTypeId { get; set; }

        public IEnumerable<Odds> HandicapOddsList { get; set; }

        public IEnumerable<Odds> OverUnderOddsList { get; set; }

        public int TeamChoosing { get; set; }

        public bool IsLive { get; set; }

        public int MatchId { get; set; }

        public DateTime KickOffTime { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool ShowLiveAndDeadball { get; set; }

        public bool IsChrome { get; set; }

        public bool IsFirefox { get; set; }

        public bool ShowHandicap { get; set; }

        public bool ShowOverUnder { get; set; }

        public float OpenSoonCountdown { get; set; }

        public int OnUserId { get; set; }

        public string HomeName
        {
            get
            {
                return CacheRef.GetTeamName(HomeId);
            }
        }

        public string AwayName
        {
            get
            {
                return CacheRef.GetTeamName(AwayId);
            }
        }

        public int MemberId { get; set; }
    }
}