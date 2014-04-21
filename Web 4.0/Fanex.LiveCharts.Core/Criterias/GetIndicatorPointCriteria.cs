// --------------------------------------------------------------------------
// <copyright file="GetIndicatorPointCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.11">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Indicator Point Criteria.
    /// </summary>
    public class GetIndicatorPointCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public int Minute { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Get_Events";
        }
    }
}