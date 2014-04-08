// --------------------------------------------------------------------------
// <copyright file="ChartAdminInsertUpdateDummyPredictionCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// TODO: Update summary.
    /// </summary>
    public class ChartAdminInsertUpdateDummyPredictionCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public int DummyCount { get; set; }

        public int AdminId { get; set; }

        public string AdminName { get; set; }

        public int ScorePredictionOptionId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Insert_Update_Dummy_Prediction";
        }

        public override bool IsValid()
        {
            return MatchId > 0 && AdminId > 0;
        }
    }
}