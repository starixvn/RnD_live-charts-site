// --------------------------------------------------------------------------
// <copyright file="Chart.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Chart Object.
    /// </summary>
    [Serializable]
    public class Chart : EntityBase
    {
        public Chart()
        {
            this.Points = new Dictionary<int, Point>();
            this.CurrentMatch = new Match();
        }

        public Match CurrentMatch { get; set; }

        public int OddsId { get; set; }

        public int OddsId1 { get; set; }

        public int BetTypeId { get; set; }

        public Dictionary<int, Point> Points { get; set; }
    }
}