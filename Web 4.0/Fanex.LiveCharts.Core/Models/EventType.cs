// --------------------------------------------------------------------------
// <copyright file="EventType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.13">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    using System;

    /// <summary>
    /// Event Type.
    /// </summary>
    [Serializable]
    public class EventType : EntityBase
    {
        public short EventTypeId { get; set; }
    }
}