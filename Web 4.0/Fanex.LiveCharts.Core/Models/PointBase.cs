// --------------------------------------------------------------------------
// <copyright file="PointBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.13">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class PointBase
    {
        public short TimeIndex { get; set; }

        public string TimeIndexString
        {
            get
            {
                if (TimeIndex >= 0)
                {
                    return TimeIndex.ToString();
                }
                else
                {
                    return string.Format("{0}:{1}", (Math.Abs(TimeIndex) / 60).ToString("00"), (Math.Abs(TimeIndex) % 60).ToString("00"));
                }
            }
        }
    }
}
