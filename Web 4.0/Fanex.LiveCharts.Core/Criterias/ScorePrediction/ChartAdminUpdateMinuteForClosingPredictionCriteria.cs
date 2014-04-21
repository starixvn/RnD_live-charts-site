// --------------------------------------------------------------------------
// <copyright file="ChartAdminUpdateMinuteForClosingPredictionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.13">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias.ScorePrediction
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Chart Admin Update Minute For Closing Prediction Criteria.
    /// </summary>
    public class ChartAdminUpdateMinuteForClosingPredictionCriteria : CriteriaBase
    {
        public int ClosePredictionMinute { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Update_Minute_For_Closing_Prediction";
        }

        public override bool IsValid()
        {
            return ClosePredictionMinute > 0;
        }
    }
}