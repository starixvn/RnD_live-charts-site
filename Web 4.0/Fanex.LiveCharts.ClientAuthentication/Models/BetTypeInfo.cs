// --------------------------------------------------------------------------
// <copyright file="BetTypeInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.11">Created</change>
// </history>

namespace Fanex.LiveCharts.ClientAuthentication.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class BetTypeInfo
    {
        public short BetTypeId { get; set; }

        public int[] OddsIds { get; set; } 
    }
}
