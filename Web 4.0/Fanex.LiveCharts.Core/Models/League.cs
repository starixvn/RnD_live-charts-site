// --------------------------------------------------------------------------
// <copyright file="League.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Models
{
    using Fanex.LiveCharts.Core;

    /// <summary>
    /// League Object.
    /// </summary>
    public class League : EntityBase
    {
        public string LeagueName
        {
            get
            {
                return CacheRef.GetLeagueName(this.Id);
            }
        }
    }
}