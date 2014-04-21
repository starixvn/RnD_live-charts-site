// --------------------------------------------------------------------------
// <copyright file="ChartInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.7">Created</change>
// </history>

namespace Fanex.LiveCharts.ClientAuthentication.Models
{
    using System;

    /// <summary>
    /// Chart Info.
    /// </summary>
    [Serializable]
    public class ChartInfo
    {
        public int MatchId { get; set; }

        public string MatchStatus { get; set; }

        public DateTime KickOffTime { get; set; }

        public BetTypeInfo[] BetTypeList { get; set; }
    }
}