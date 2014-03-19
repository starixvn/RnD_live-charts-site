// --------------------------------------------------------------------------
// <copyright file="Match.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.LiveCharts.Core;

    /// <summary>
    /// Match object.
    /// </summary>
    [Serializable]
    public class Match : EntityBase
    {
        public Match()
        {
            OddsList = new List<Odds>();
        }

        public bool IsLive { get; set; }

        public DateTime KickOffTime { get; set; }

        public string KickOffTimeString
        {
            get
            {
                return KickOffTime.ToString(Constants.FORMAT_DATE_TIME);
            }
        }

        public string KickOffTimeGMT8
        {
            get
            {
                return KickOffTime.ToGMT8();
            }
        }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

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

        public string MatchName
        {
            get
            {
                return string.Format("{0} vs {1}", CacheRef.GetTeamName(HomeId), CacheRef.GetTeamName(AwayId));
            }
        }

        public bool CanEnableChart { get; set; }

        public bool HideInSearch { get; set; }

        public bool ChartEnabled { get; set; }

        public byte ChartShowingStatus { get; set; }

        public List<Odds> OddsList { get; set; }
    }
}