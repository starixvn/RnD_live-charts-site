// --------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Core
{
    using System;
    using System.Configuration;
    using Fanex.Common;
    using Fanex.Logging;

    /// <summary>
    /// Load setting from web config.
    /// </summary>
    public class AppSettings
    {
        static AppSettings()
        {
            try
            {
                RedisHost = GetValue("RedisHost");
                RedisPort = Converter.ToInt(GetValue("RedisPort"));
                CurrentCache = (CacheType)Converter.ToInt(GetValue("CacheType"));
                EnableAllTeamChoice = Converter.ToInt(GetValue("EnableAllTeamChoice")) > 0;
                LocalTest = Converter.ToInt(GetValue("LocalTest")) > 0;
                IsAdminSite = Converter.ToInt(GetValue("IsAdminSite")) > 0;
                RedirectURLToAdminSite = GetValue("RedirectURLToAdminSite");
                EnableLogAuthentication = Converter.ToInt(GetValue("EnableLogAuthentication")) > 0;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }
        }

        public static string RedisHost { get; set; }

        public static int RedisPort { get; set; }

        public static CacheType CurrentCache { get; set; }

        public static bool EnableAllTeamChoice { get; set; }

        public static bool LocalTest { get; set; }

        public static bool IsAdminSite { get; set; }

        public static string RedirectURLToAdminSite { get; set; }

        public static bool EnableLogAuthentication { get; set; }

        /// <summary>
        /// Get value from key string.
        /// </summary>
        /// <param name="key">Setting key name.</param>
        /// <returns>Setting value.</returns>
        public static string GetValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return string.Empty;
        }
    }
}