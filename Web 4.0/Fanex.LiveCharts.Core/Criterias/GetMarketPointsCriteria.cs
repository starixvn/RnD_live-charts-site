// --------------------------------------------------------------------------
// <copyright file="GetMarketPointsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.13">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    /// <summary>
    /// Get Market points criteria.
    /// </summary>
    public class GetMarketPointsCriteria : GetPointsCriteria
    {
        public int Odds1Id { get; set; }
    }
}