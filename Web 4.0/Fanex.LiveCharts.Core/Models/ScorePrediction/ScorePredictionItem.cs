// --------------------------------------------------------------------------
// <copyright file="ScorePredictionItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Models.ScorePrediction
{
    using System;

    /// <summary>
    /// Score Prediction Item.
    /// </summary>
    public class ScorePredictionItem
    {
        public int ScorePredictionId { get; set; }

        public string MemberId { get; set; }

        public int ScorePredictionOptionId { get; set; }

        public DateTime ScorePredicitonTime { get; set; }

        public int MatchId { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }
    }
}