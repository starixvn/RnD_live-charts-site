// --------------------------------------------------------------------------
// <copyright file="UpdateOddsStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.8.21">Created</change>
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
    public class UpdateOddsStatusCriteria : CriteriaBase
    {
        public int OddsId { get; set; }

        public bool EnableChart { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Update_Odds_Status";
        }

        public override bool IsValid()
        {
            return OddsId > 0;
        }
    }
}
