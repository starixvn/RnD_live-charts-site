// --------------------------------------------------------------------------
// <copyright file="RedisCachingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Impl
{
    using System;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Services.Interface;
    using ServiceStack.Redis;

    /// <summary>
    /// Caching service.
    /// </summary>
    public class RedisCachingService : ServiceBase, ICachingService
    {
        /// <summary>
        /// Get data from cache.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="key">Key in cache.</param>
        /// <returns>Cache data.</returns>
        public T Get<T>(string key)
        {
            try
            {
                using (var conn = new RedisClient(AppSettings.RedisHost, AppSettings.RedisPort))
                {
                    return conn.Get<T>(key);
                }
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
        /// <typeparam name="T">Type of data.</typeparam>
        /// <param name="key">Key set in cache.</param>
        /// <param name="value">Value set to cache.</param>
        /// <returns>Save in cache successful or not.</returns>
        public bool Set<T>(string key, T value)
        {
            try
            {
                using (var conn = new RedisClient(AppSettings.RedisHost, AppSettings.RedisPort))
                {
                    return conn.Set<T>(key, value);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return false;
            }
        }

        /// <summary>
        /// Set data to cache.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="key">Key for set cache.</param>
        /// <param name="value">Value for set cache.</param>
        /// <param name="timeSpanExpiration">Expire after timespan.</param>
        /// <returns>Save in cache success or not.</returns>
        public bool Set<T>(string key, T value, TimeSpan timeSpanExpiration)
        {
            try
            {
                using (var conn = new RedisClient(AppSettings.RedisHost, AppSettings.RedisPort))
                {
                    return conn.Set<T>(key, value, timeSpanExpiration);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return false;
            }
        }

        /// <summary>
        /// Set data to cache.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="key">Key for set cache.</param>
        /// <param name="value">Value for set cache.</param>
        /// <param name="absoluteExpiration">Expire to time.</param>
        /// <returns>Save in cache success or not.</returns>
        public bool Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            try
            {
                using (var conn = new RedisClient(AppSettings.RedisHost, AppSettings.RedisPort))
                {
                    return conn.Set<T>(key, value, absoluteExpiration);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return false;
            }
        }

        /// <summary>
        /// Remove key from cache.
        /// </summary>
        /// <param name="key">Key name for remove.</param>
        public void Remove(string key)
        {
            try
            {
                using (var conn = new RedisClient(AppSettings.RedisHost, AppSettings.RedisPort))
                {
                    conn.Remove(key);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
    }
}