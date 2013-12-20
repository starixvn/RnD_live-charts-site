// --------------------------------------------------------------------------
// <copyright file="UpdateChartShowingStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.11.29">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateChartShowingStatusCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public byte ChartShowingStatus { get; set; } 

        public override string GetSettingKey()
        {
            return "Chart_Admin_Update_Chart_Showing_Status";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}
