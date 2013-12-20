// -----------------------------------------------------------------------
// <copyright file="ChartInfo.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCharts.ClientAuthentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class ChartInfo
    {
        public int MatchId { get; set; }
        public short BetTypeId { get; set; }
        public int OddsId { get; set; }
    }
}
