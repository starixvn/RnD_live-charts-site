// --------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.26">Created</change>
// </history>

namespace Fanex.LiveCharts.ViewModel
{
    /// <summary>
    /// Main Page View Model.
    /// </summary>
    public class MainPageViewModel
    {
        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool AnonymousLogin { get; set; }

        public bool IsMatchCompleted { get; set; }
    }
}