// --------------------------------------------------------------------------
// <copyright file="InsertEventCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class InsertEventCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public int TeamId { get; set; }

        public short EventTypeId { get; set; }

        public int Half { get; set; }

        public short Minute { get; set; }

        public short Second { get; set; }

        [SpParam(SqlParamName = "value")]
        public string Detail { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Insert_Event";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}