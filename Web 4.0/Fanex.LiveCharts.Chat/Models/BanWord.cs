// --------------------------------------------------------------------------
// <copyright file="BanWord.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.30">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BanWord
    {
        public int BanWordId { get; set; }

        public string BanWordText { get; set; }

        public bool DeepChecking { get; set; }

        public bool Deleted { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
