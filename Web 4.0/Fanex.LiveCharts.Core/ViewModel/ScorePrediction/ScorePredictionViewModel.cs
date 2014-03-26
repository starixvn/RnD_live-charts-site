// --------------------------------------------------------------------------
// <copyright file="ScorePredictionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.26">Created</change>
// </history>

namespace Fanex.LiveCharts.ViewModel.ScorePrediction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.LiveCharts.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScorePredictionViewModel
    {
        public Match Match { get; set; }
        public int MemberId { get; set; }
        public bool IsStopScorePrediction { get; set; }
    }
}
