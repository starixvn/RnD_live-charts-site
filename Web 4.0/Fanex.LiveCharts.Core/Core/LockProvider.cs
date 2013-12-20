// --------------------------------------------------------------------------
// <copyright file="LockProvider.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Lock Provider.
    /// </summary>
    /// <typeparam name="T">T is class.</typeparam>
    public class LockProvider<T>
    {
        private object _syncRoot = new object();
        private Dictionary<T, object> _dicLocks = new Dictionary<T, object>();

        /// <summary>
        /// Gets an object suitable for locking the specified data item.
        /// </summary>
        /// <param name="key">The data key.</param>
        /// <returns>Lock object.</returns>
        public object GetLock(T key)
        {
            if (!_dicLocks.ContainsKey(key))
            {
                lock (_syncRoot)
                {
                    if (!_dicLocks.ContainsKey(key))
                    {
                        _dicLocks[key] = new object();
                    }
                }
            }

            return _dicLocks[key];
        }

        /// <summary>
        /// Remove a lock object from lock dictionary.
        /// </summary>
        /// <param name="key">Dictionary key.</param>
        public void RemoveLock(T key)
        {
            lock (_syncRoot)
            {
                if (_dicLocks.ContainsKey(key))
                {
                    _dicLocks.Remove(key);
                }
            }
        }

        /// <summary>
        /// Check whether dictionary contains key or not.
        /// </summary>
        /// <param name="key">Key need be checked.</param>
        /// <returns>Contains or not.</returns>
        public bool ContainsKey(T key)
        {
            lock (_syncRoot)
            {
                return _dicLocks.ContainsKey(key) && _dicLocks[key] != null;
            }
        }

        public void RemoveAllLocks()
        {
            lock (_syncRoot)
            {
                _dicLocks = new Dictionary<T, object>();
            }
        }
    }
}