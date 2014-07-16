// --------------------------------------------------------------------------
// <copyright file="GetChartsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.4">Created</change>
// </history>

namespace Fanex.LiveCharts.Authentication.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Charts Criteria.
    /// </summary>
    public class GetChartsCriteria : CriteriaBase
    {
        public int SportId { get; set; }

        public DateTime KickOffDate { get; set; }

        public int TimeZone { get; set; }

        public string MatchStatus { get; set; }

        public byte ChartShowingStatus { get; set; }

        public byte EarlyDay { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Get_Charts";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}