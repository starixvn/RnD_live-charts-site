// --------------------------------------------------------------------------
// <copyright file="CouchBaseCachingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.4">Created</change>
// </history>

namespace Fanex.LiveCharts.Authentication.Core
{
    using System;
    using Fanex.Caching;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CouchBaseCachingService
    {
        private static ICache _cacheManager;
        private static object _lockSetupCache = new object();

        public static ICache CacheClient
        {
            get
            {
                if (_cacheManager == null)
                {
                    lock (_lockSetupCache)
                    {
                        _cacheManager = CacheService.CreateInstance(Fanex.Caching.CacheType.MemCache);
                    }
                }

                return _cacheManager;
            }
        }

        /// <summary>
        /// Get data from cache.
        /// </summary>
        /// <typeparam name="T">Object type get from cache.</typeparam>
        /// <param name="key">Key for get data.</param>
        /// <returns>Object data.</returns>
        public static T Get<T>(string key)
        {
            try
            {
                return CacheClient.Get<T>(key);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return default(T);
            }
        }

        /// <summary>
        /// Set data to cache.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="key">Key for set data.</param>
        /// <param name="value">Value need save in cache.</param>
        /// <returns>Save success or not.</returns>
        public static bool Set<T>(string key, T value)
        {
            try
            {
                CacheClient.Set<T>(key, value);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Set data to cache.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="key">Key for set cache.</param>
        /// <param name="value">Value for set cache.</param>
        /// <param name="timeSpanExpiration">Expire after timespan.</param>
        /// <returns>Save in cache success or not.</returns>
        public static bool Set<T>(string key, T value, TimeSpan timeSpanExpiration)
        {
            try
            {
                CacheClient.Set<T>(key, value, timeSpanExpiration);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Set data to cache.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="key">Key for set cache.</param>
        /// <param name="value">Value for set cache.</param>
        /// <param name="absoluteExpiration">Expire to time.</param>
        /// <returns>Save in cache success or not.</returns>
        public static bool Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            try
            {
                CacheClient.Set<T>(key, value, absoluteExpiration);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Remove key from cache.
        /// </summary>
        /// <param name="key">Key name for get data.</param>
        public static void Remove(string key)
        {
            CacheClient.Remove(key);
        }

        /// <summary>
        /// Log exception for service.
        /// </summary>
        /// <param name="ex">Exception object.</param>
        private static void ExceptionHandler(Exception ex)
        {
            Logger.Log.Error(ex.Message, ex);
        }
    }
}