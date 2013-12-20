// --------------------------------------------------------------------------
// <copyright file="GetOddsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Odds criteria.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public class GetOddsCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public int BetTypeId { get; set; }

        public bool IsLive { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Get_Odds";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}