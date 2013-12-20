// --------------------------------------------------------------------------
// <copyright file="Odds.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    using System;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Resources;

    /// <summary>
    /// Odds object.
    /// </summary>
    [Serializable]
    public class Odds : EntityBase
    {
        public short BetTypeId { get; set; }

        public DateTime ChangeTime { get; set; }

        public string ChangeTimeString 
        { 
            get 
            { 
                return ChangeTime.ToString(Constants.FORMAT_DATE_TIME); 
            } 
        }

        public bool IsLive { get; set; }

        public double HomeHandicap { get; set; }

        public double AwayHandicap { get; set; }

        public string HomeHandicapString
        {
            get
            {
                return GetHandicapString(HomeHandicap, BetTypeId == (int)BetTypeEnum.Handicap ? "Home" : "OU");
            }
        }

        public string AwayHandicapString
        {
            get
            {
                return GetHandicapString(AwayHandicap, BetTypeId == (int)BetTypeEnum.Handicap ? "Away" : "OU");
            }
        }

        public string OddsName
        {
            get
            {
                ////var oddsName = IsLive ? "LIVE - " : "NON-LIVE - ";
                var oddsName = AwayHandicap > 0 ? AwayHandicapString : HomeHandicapString;
                return oddsName;
            }
        }

        public string OddsInformationString
        {
            get
            {
                var betTypeText = string.Empty;
                switch (BetTypeId)
                {
                    case (int)BetTypeEnum.Handicap:
                        betTypeText = Lang.Handicap;
                        break;
                    case (int)BetTypeEnum.OverUnder:
                        betTypeText = Lang.OverUnder;
                        break;
                }

                return string.Format("{0}: {1} - {2}", betTypeText, Id, AwayHandicap > 0 ? AwayHandicapString : HomeHandicapString);
            }
        }

        public bool ChartStatus { get; set; }

        /// <summary>
        /// Get handicap string from handicap value.
        /// </summary>
        /// <param name="handicap">Handicap value.</param>
        /// <param name="choice">Team choice.</param>
        /// <returns>Handicap string formatted.</returns>
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