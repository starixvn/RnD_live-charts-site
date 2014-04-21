// --------------------------------------------------------------------------
// <copyright file="AuthenticationController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.4">Created</change>
// </history>

namespace Fanex.LiveCharts.Authentication.Controllers
{
    using System;
    using System.Web.Http;
    using Fanex.LiveCharts.Authentication.Services;
    using Fanex.LiveCharts.ClientAuthentication.Models;
    using Fanex.Logging;

    /// <summary>
    /// Authentication Controller.
    /// </summary>
    public class AuthenticationController : ApiController
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController()
        {
            _authenticationService = new AuthenticationService();
        }

        public string GetToken(int memberId, string encryptText, int matchId)
        {
            return _authenticationService.GetToken(memberId, encryptText, matchId);
        }

        /// <summary>
        /// Get Chart information.
        /// </summary>
        /// <param name="kickOffDate">Kick off date of match.</param>
        /// <param name="sportId">Sport Type of match.</param>
        /// <param name="timeZone">Time zone of kick off date.</param>
        /// <param name="matchStatus">Match status for query.</param>
        /// <param name="chartShowingStatus">Chart can be show in production or both production and QA site.</param>
        /// <returns>Array of Chart.</returns>
        public ChartInfo[] GetCharts(
            DateTime kickOffDate,
            int sportId = 0,
            int timeZone = -4,
            string matchStatus = "all",
            byte chartShowingStatus = 1)
        {
            try
            {
                ChartInfo[] chart = _authenticationService.GetCharts(
                    kickOffDate,
                    sportId,
                    timeZone,
                    matchStatus,
                    chartShowingStatus);

                return chart;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return new ChartInfo[0];
        }
    }
}