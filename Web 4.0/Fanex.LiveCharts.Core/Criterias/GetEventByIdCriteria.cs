// -----------------------------------------------------------------------
// <copyright file="GetEventByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.7.24">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Event By Id Criteria.
    /// </summary>
    public class GetEventByIdCriteria : CriteriaBase
    {
        public long EventId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Get_Event_By_Id";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}