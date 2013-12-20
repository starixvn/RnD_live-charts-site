// --------------------------------------------------------------------------
// <copyright file="Point.cs" company="Nexcel Solutions Vietnam">
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
    /// Point object, contains all information about display point.
    /// </summary>
    [Serializable]
    public class Point : PointBase
    {
        public Point()
        {
            SubPoints = new List<Point>();
        }

        public string OpenTime { get; set; }

        public short BetTypeId { get; set; }

        public string Time
        {
            get
            {
                return RecordTime.ToString(Constants.FORMAT_DATE_TIME);
            }
        }

        public double HomeHandicap { get; set; }

        public double AwayHandicap { get; set; }

        public string HomeHandicapString
        {
            get
            {
                return GetHandicapString(HomeHandicap, BetTypeId == (int)BetTypeEnum.Handicap ? "Home" : "Over");
            }
        }

        public string AwayHandicapString
        {
            get
            {
                return GetHandicapString(AwayHandicap, BetTypeId == (int)BetTypeEnum.Handicap ? "Away" : "Under");
            }
        }

        public int HomeTickets { get; set; }

        public int AwayTickets { get; set; }

        public int DrawTickets { get; set; }

        public int TotalTickets
        {
            get
            {
                return this.HomeTickets + this.DrawTickets + this.AwayTickets;
            }
        }

        public double HomeOdds { get; set; }

        public double AwayOdds { get; set; }

        public double DrawOdds { get; set; }

        public DateTime RecordTime { get; set; }

        public string RecordTimeString
        {
            get
            {
                return RecordTime.ToString(Constants.FORMAT_DATE_TIME);
            }
        }

        public string Name { get; set; }

        public IndicatorType Type { get; set; }

        public int TeamId { get; set; }

        public string Value { get; set; }

        public short EventType { get; set; }

        public IList<Point> SubPoints { get; set; }

        /// <summary>
        /// Get handicap string from handicap value and choice.
        /// </summary>
        /// <param name="handicap">Handicap value.</param>
        /// <param name="choice">Choice team.</param>
        /// <returns>Handicap string.</returns>
        private string GetHandicapString(double handicap, string choice)
        {
            if (BetTypeId == (int)BetTypeEnum.Handicap || BetTypeId == (int)BetTypeEnum.OverUnder)
            {
                if ((handicap * 10) % 5 == 0)
                {
                    return string.Format("{0} {1}", choice, handicap);
                }
                else
                {
                    return string.Format("{0} {1}-{2}", choice, handicap - 0.25, handicap + 0.25);
                }
            }

            return string.Empty;
        }
    }
}