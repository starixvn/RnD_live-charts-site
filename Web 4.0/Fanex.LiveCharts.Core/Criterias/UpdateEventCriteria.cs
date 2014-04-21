// -----------------------------------------------------------------------
// <copyright file="UpdateEventCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.7.24">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Event Criteria.
    /// </summary>
    public class UpdateEventCriteria : CriteriaBase
    {
        public int EventId { get; set; }

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
            return "Chart_Admin_Update_Event";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}