// --------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Fanex.LiveCharts.Models;

    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Convert an enumerable to dictionary.
        /// </summary>
        /// <param name="currentEnumerable">Enumerable need be converted.</param>
        /// <returns>Dictionary after converted.</returns>
        public static Dictionary<int, Point> ToDictionary(this IEnumerable<Point> currentEnumerable)
        {
            Dictionary<int, Point> toDic = new Dictionary<int, Point>();
            foreach (var item in currentEnumerable)
            {
                toDic.Add(item.TimeIndex, item);
            }

            return toDic;
        }
    }
}