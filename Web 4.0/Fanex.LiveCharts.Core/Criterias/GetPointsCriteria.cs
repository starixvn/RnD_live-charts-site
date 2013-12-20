// --------------------------------------------------------------------------
// <copyright file="GetPointsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCharts.Core;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Point Criteria.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public class GetPointsCriteria : CriteriaBase
    {
        public int OddsId { get; set; }

        public int MatchId { get; set; }

        public int ChartId { get; set; }

        public int BetType { get; set; }

        public int Minute { get; set; }

        public DateTime MaxTime { get; set; }

        /// <summary>
        /// Get Setting key for query database.
        /// </summary>
        /// <returns>Current store procedure name.</returns>
        public override string GetSettingKey()
        {
            switch (ChartId)
            {
                case (int)ChartType.Price:
                    return "Chart_Get_Odds_Data";
                case (int)ChartType.Market:
                    return "Chart_Get_Tickets_Data";
                case (int)ChartType.Event:
                    return "Chart_Get_Events";
            }

            return string.Empty;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}