// --------------------------------------------------------------------------
// <copyright file="UpdateMatchStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.13">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public class UpdateMatchStatusCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public bool ChartEnable { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Update_Match_Status";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}