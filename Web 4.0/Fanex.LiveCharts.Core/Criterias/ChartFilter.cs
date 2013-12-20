// --------------------------------------------------------------------------
// <copyright file="ChartFilter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System;
    using Fanex.LiveCharts.Core;

    /// <summary>
    /// Chart filter.
    /// </summary>
    public class ChartFilter
    {
        public int Interval { get; set; }

        public int PriceOddsChartTimeIndex { get; set; }

        public int MarketChartTimeIndex { get; set; }

        public int EventChartTimeIndex { get; set; }

        public int MatchId { get; set; }

        public int BetTypeId { get; set; }

        public int OddsId { get; set; }

        public bool IsLive { get; set; }

        public int ChartId { get; set; }

        public int TeamChoosing { get; set; }

        public string OddsIds { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime KickOffTime { get; set; }

        public int CurrentTimeIndex
        {
            get
            {
                switch (ChartId)
                {
                    case (int)ChartType.Market:
                        return MarketChartTimeIndex;
                    case (int)ChartType.Price:
                        return PriceOddsChartTimeIndex;
                    case (int)ChartType.Event:
                        return EventChartTimeIndex;
                }

                return 0;
            }
        }

        public int DefaultStartTime
        {
            get
            {
                return IsLive ? Constants.LIVE_START_MINUTE : Constants.DEAD_BALL_START_MINUTE;
            }
        }

        public int DefaultStopTime
        {
            get
            {
                return IsLive ? Constants.LIVE_STOP_MINUTE : Constants.DEAD_BALL_STOP_MINUTE;
            }
        }

        public int ChartInterval
        {
            get
            {
                return IsLive ? Constants.LIVE_MATCH_INTERVAL : Constants.DEAD_BALL_MATCH_INTERVAL;
            }
        }

        /// <summary>
        /// Clone a Chart filter for query data.
        /// </summary>
        /// <param name="charType">Chart type need clone.</param>
        /// <returns>New chart filter.</returns>
        public ChartFilter Clone(ChartType charType)
        {
            return new ChartFilter
            {
                BetTypeId = BetTypeId,
                Interval = Interval,
                IsLive = IsLive,
                KickOffTime = KickOffTime,
                MarketChartTimeIndex = MarketChartTimeIndex,
                MatchId = MatchId,
                OddsId = OddsId,
                OpenDate = OpenDate,
                PriceOddsChartTimeIndex = PriceOddsChartTimeIndex,
                ChartId = (int)charType
            };
        }
    }
}