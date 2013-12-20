// --------------------------------------------------------------------------
// <copyright file="CurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Web
{
    using System;

    /// <summary>
    /// Current session data.
    /// </summary>
    [Serializable]
    public class CurrentSession
    {
        public int MemberId { get; set; }

        public int MatchId { get; set; }

        public string Token { get; set; }

        public int AdminId { get; set; }

        public string AdminName { get; set; }

        public string Language { get; set; }
    }
}