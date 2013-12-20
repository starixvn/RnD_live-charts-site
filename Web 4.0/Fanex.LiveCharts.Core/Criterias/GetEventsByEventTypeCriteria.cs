// --------------------------------------------------------------------------
// <copyright file="GetEventsByEventTypeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.8.2">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Events by event type.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public class GetEventsByEventTypeCriteria : CriteriaBase
    {
        public short EventType { get; set; }

        public int MatchId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Get_Events_By_Event_Type";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}