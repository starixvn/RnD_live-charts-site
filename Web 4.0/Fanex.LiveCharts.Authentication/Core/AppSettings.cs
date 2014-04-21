// --------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.4">Created</change>
// </history>

namespace Fanex.LiveCharts.Authentication.Core
{
    using System;
    using System.Configuration;
    using Fanex.Common;
    using Fanex.Logging;

    /// <summary>
    /// App Settings.
    /// </summary>
    public class AppSettings
    {
        static AppSettings()
        {
            try
            {
                CachingTimeOutMinute = Converter.ToInt(GetValue("CachingTimeOutMinute"));
                CachingGetChartsTimeOutSeconds = Converter.ToInt(GetValue("CachingGetChartsTimeOutSeconds"));
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }
        }

        public static int CachingTimeOutMinute { get; set; }

        public static int CachingGetChartsTimeOutSeconds { get; set; }

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