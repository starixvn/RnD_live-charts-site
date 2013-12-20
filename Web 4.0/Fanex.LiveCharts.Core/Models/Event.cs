// --------------------------------------------------------------------------
// <copyright file="Event.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.8.20">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    using System;
    using Fanex.LiveCharts.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class Event : EntityBase
    {
        public long EventId { get; set; }

        public int Half { get; set; }

        public short Minute { get; set; }

        public short Second { get; set; }

        public short EventTypeId { get; set; }

        public int TeamId { get; set; }

        public string TeamName
        {
            get
            {
                return CacheRef.GetTeamName(TeamId);
            }
        }

        public string Detail { get; set; }
    }
}