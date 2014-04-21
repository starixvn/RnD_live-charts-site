// --------------------------------------------------------------------------
// <copyright file="GetUserPredictionCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get User Prediction Criteria.
    /// </summary>
    public class GetUserPredictionCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public string MemberId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Get_User_Prediction";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}