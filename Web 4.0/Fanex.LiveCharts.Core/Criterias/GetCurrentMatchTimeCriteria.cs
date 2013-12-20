// --------------------------------------------------------------------------
// <copyright file="GetCurrentMatchTimeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.11.8">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Current Live Match Time.
    /// </summary>
    public class GetCurrentMatchTimeCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public int Minute { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Get_Current_Match_Data";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}