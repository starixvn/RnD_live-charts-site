// --------------------------------------------------------------------------
// <copyright file="CurrentMatchTime.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.11.8">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    /// <summary>
    /// Get Current Information of Match.
    /// </summary>
    public class CurrentMatchTime
    {
        public int CurrentMinute { get; set; }

        public short HomeScore { get; set; }

        public short AwayScore { get; set; }

        public byte LivePeriod { get; set; }
    }
}