// --------------------------------------------------------------------------
// <copyright file="IsMatchCompleteCaptureCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.7">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IsMatchCompleteCaptureCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Is_Match_Complete_Capture";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}