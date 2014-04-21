// --------------------------------------------------------------------------
// <copyright file="ChartShowingType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.12.2">Created</change>
// </history>

namespace Fanex.LiveCharts.ClientAuthentication.Core
{
    /// <summary>
    /// Chart Showing Type.
    /// </summary>
    public enum ChartShowingType : byte
    {
        /// <summary>
        /// Show both production and QA site.
        /// </summary>
        ShowOnProAndQASite = 1,

        /// <summary>
        /// Show just only in production.
        /// </summary>
        ShowOnlyOnQASite = 0
    }
}
