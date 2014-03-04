// --------------------------------------------------------------------------
// <copyright file="ScorePredictionResultViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.4">Created</change>
// </history>

namespace Fanex.LiveCharts.ViewModel.ScorePredictionViewModel
{
    /// <summary>
    /// View model contains information about score prediction result.
    /// </summary>
    public class ScorePredictionResultViewModel
    {
        public int MatchId { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }
    }
}