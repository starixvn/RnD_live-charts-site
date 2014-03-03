// --------------------------------------------------------------------------
// <copyright file="InsertOrUpdateScorePredictionItemCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias.ScorePrediction
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InsertOrUpdateScorePredictionItemCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public string MemberId { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Insert_Or_Update_Score_Prediction_Item";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}