// --------------------------------------------------------------------------
// <copyright file="OddsGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.LiveCharts.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OddsGroup
    {
        public int BetTypeId { get; set; }

        public IList<Odds> OddsList { get; set; }

        public bool AllOddsEnable
        {
            get
            {
                if (OddsList != null && OddsList.Any())
                {
                    var disableOdds = OddsList.Where(o => !o.ChartStatus);
                    return !disableOdds.Any();
                }

                return false;
            }
        }

        public string BetTypeName
        {
            get
            {
                return CacheRef.GetBettype(BetTypeId);
            }
        }
    }
}