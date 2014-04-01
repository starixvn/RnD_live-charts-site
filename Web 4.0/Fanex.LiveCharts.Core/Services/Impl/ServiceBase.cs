// --------------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Impl
{
    using System;
    using System.Web;
    using Fanex.Logging;

    /// <summary>
    /// Base service.
    /// </summary>
    public class ServiceBase
    {
        /// <summary>
        /// Log exception for service.
        /// </summary>
        /// <param name="ex">Exception object.</param>
        public static void ExceptionHandler(Exception ex)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                string message = ex.Message +
                            "\r\nSource: " + ex.Source +
                            "\r\nDomain: " + HttpContext.Current.Request.Headers["Host"] +
                            "\r\nForm: " + HttpContext.Current.Request.Form.ToString() +
                            "\r\nQueryString: " + HttpContext.Current.Request.QueryString.ToString() +
                            "\r\nTargetSite: " + ex.TargetSite +
                            "\r\nStackTrace: " + ex.StackTrace;

                Logger.Log.Error(message);
            }
            else
            {
                Logger.Log.Error(ex.Message, ex);
            }
        }

        public static void LogInformation(string message)
        {
            Logger.Log.Info(message);
        }
    }
}