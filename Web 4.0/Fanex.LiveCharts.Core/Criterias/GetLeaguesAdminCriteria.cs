// --------------------------------------------------------------------------
// <copyright file="GetLeaguesAdminCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.13">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public class GetLeaguesAdminCriteria : CriteriaBase
    {
        public DateTime MatchDate { get; set; }

        public int SportId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Get_Leagues";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}