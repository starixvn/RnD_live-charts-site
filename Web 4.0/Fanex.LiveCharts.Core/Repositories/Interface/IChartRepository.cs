// --------------------------------------------------------------------------
// <copyright file="IChartRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Repositories.Interface
{
    using System;
    using System.Collections.Generic;
    using Fanex.LiveCharts.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Chart repository interface.
    /// </summary>
    public interface IChartRepository : IBaseRepository
    {
        /// <summary>
        /// Get job max time.
        /// </summary>
        /// <param name="criteria">Get max time criteria.</param>
        /// <returns>Current job max time.</returns>
        DateTime? GetMaxTime(ICriteria criteria);

        /// <summary>
        /// Fetch data from database.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="criteria">Get data criteria.</param>
        /// <param name="maxTime">Get output max time.</param>
        /// <returns>Data list.</returns>
        IEnumerable<T> Fetch<T>(ICriteria criteria, ref DateTime maxTime);

        IEnumerable<Match> GetEnableCharts(ICriteria criteria);

        CurrentMatchTime GetCurrentMatchTime(ICriteria criteria);
    }
}