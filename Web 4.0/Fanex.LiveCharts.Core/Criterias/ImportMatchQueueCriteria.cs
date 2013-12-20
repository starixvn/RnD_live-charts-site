// --------------------------------------------------------------------------
// <copyright file="ImportMatchQueueCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.8.5">Created</change>
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
    public class ImportMatchQueueCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public byte ChartShowingStatus { get; set; }

        [SpParam(Ignored = true)]
        public DateTime KickOffTime { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Match_Queue_Import";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}