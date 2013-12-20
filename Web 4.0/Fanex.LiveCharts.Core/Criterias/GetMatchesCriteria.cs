// --------------------------------------------------------------------------
// <copyright file="GetMatchesCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Matches criteria.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public class GetMatchesCriteria : CriteriaBase
    {
        public int LeagueId { get; set; }

        public DateTime MatchDate { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Get_Matches";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}