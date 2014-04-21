// --------------------------------------------------------------------------
// <copyright file="RemoveLockObjectService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.8">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Impl
{
    using System;
    using System.Threading;
    using Fanex.LiveCharts.Core;

    /// <summary>
    /// Remove Lock Object Service.
    /// </summary>
    public class RemoveLockObjectService : ServiceBase
    {
        private Thread _workerThread;
        private int _sleepTime;

        public RemoveLockObjectService(int sleepTimeMinutes = 720)
        {
            _sleepTime = sleepTimeMinutes * 60 * 1000;
            _workerThread = new Thread(Run);
            Start();
        }

        protected bool IsStop { get; set; }

        /// <summary>
        /// Start Worker.
        /// </summary>
        public void Start()
        {
            try
            {
                if (_workerThread != null)
                {
                    _workerThread.Start();
                    IsStop = false;
                }
                else
                {
                    ExceptionHandler(new Exception("Cannot start remove lock object service."));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Set Stop Worker.
        /// </summary>
        public void Stop()
        {
            IsStop = true;
        }

        /// <summary>
        /// Run main function.
        /// </summary>
        private void Run()
        {
            Thread.Sleep(_sleepTime);
            while (true)
            {
                try
                {
                    Constants.LockGetPoints.RemoveAllLocks();
                    Constants.LockGetCache.RemoveAllLocks();
                }
                catch (Exception ex)
                {
                    ExceptionHandler(ex);
                }
            }
        }
    }
}