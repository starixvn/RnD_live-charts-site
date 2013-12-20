// --------------------------------------------------------------------------
// <copyright file="CacheManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Impl
{
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Services.Interface;

    /// <summary>
    /// Handle cache in application.
    /// </summary>
    public class CacheManager
    {
        /// <summary>
        /// Initializes static members of the <see cref="CacheManager" /> class.
        /// </summary>
        static CacheManager()
        {
            if (AppSettings.CurrentCache == CacheType.Redis)
            {
                Instance = new RedisCachingService();
            }
            else
            {
                Instance = new CouchBaseCachingService();
            }
        }

        public static ICachingService Instance { get; set; }
    }
}