// --------------------------------------------------------------------------
// <copyright file="GetScorePredictionOptionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.11">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias.ScorePrediction
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Score Prediction Option Criteria.
    /// </summary>
    public class GetScorePredictionOptionCriteria : CriteriaBase
    {
        public int ScorePredictionOptionId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Get_Score_Prediction_Option";
        }

        public override bool IsValid()
        {
            return ScorePredictionOptionId > 0;
        }
    }
}