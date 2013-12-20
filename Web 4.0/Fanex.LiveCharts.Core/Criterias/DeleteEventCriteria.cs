// --------------------------------------------------------------------------
// <copyright file="DeleteEventCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.8.20">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public class DeleteEventCriteria : CriteriaBase
    {
        public long EventId { get; set; }

        public bool IsDeleted { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Delete_Event";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}