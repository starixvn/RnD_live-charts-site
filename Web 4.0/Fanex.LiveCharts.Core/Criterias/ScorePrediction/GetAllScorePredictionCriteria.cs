// --------------------------------------------------------------------------
// <copyright file="GetAllScorePredictionCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get All Score Prediction Criteria.
    /// </summary>
    public class GetAllScorePredictionCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        [SpParam(Ignored = true)]
        public bool IsAdminSite { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Get_All_Score_Prediction";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}