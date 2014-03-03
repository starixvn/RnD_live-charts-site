// --------------------------------------------------------------------------
// <copyright file="ScorePredictionTotal.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Models.ScorePrediction
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScorePredictionTotal
    {
        public ScorePredictionTotal()
        {
            ScorePredictionOptionTotal = new List<ScorePredictionOption>();
        }

        public IList<ScorePredictionOption> ScorePredictionOptionTotal { get; set; }

        public int TotalPrediction { get; set; }
    }
}