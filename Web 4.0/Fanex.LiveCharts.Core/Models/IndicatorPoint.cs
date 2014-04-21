// --------------------------------------------------------------------------
// <copyright file="IndicatorPoint.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.11">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    using Fanex.LiveCharts.Core;

    /// <summary>
    /// Indicator Point.
    /// </summary>
    public class IndicatorPoint : PointBase
    {
        public string Name { get; set; }

        public IndicatorType Type { get; set; }

        public int TeamId { get; set; }

        public string Value { get; set; }

        public short EventType { get; set; }
    }
}