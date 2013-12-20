// --------------------------------------------------------------------------
// <copyright file="ChartFlat.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.11">Created</change>
// </history>

namespace Fanex.LiveCharts.Authentication.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChartFlat
    {
        public int MatchId { get; set; }

        public string MatchStatus { get; set; }

        public short BetTypeId { get; set; }

        public int OddsId { get; set; }

        public DateTime KickOffTime { get; set; }
    }
}
