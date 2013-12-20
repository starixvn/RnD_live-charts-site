// --------------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.7">Created</change>
// </history>

namespace Fanex.LiveCharts.Authentication.Services
{
    using System;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServiceBase : DynamicRepository
    {
        /// <summary>
        /// Log exception for service.
        /// </summary>
        /// <param name="ex">Exception object.</param>
        protected void ExceptionHandler(Exception ex)
        {
            Logger.Log.Error(ex.Message, ex);
        }
    }
}