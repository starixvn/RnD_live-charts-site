// --------------------------------------------------------------------------
// <copyright file="ScorePredictionOption.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Models.ScorePrediction
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScorePredictionOption
    {
        public int ScorePredictionOptionId { get; set; }

        public int MatchId { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public int ScorePredictionCount { get; set; }
    }
}