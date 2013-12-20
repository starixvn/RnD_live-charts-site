// --------------------------------------------------------------------------
// <copyright file="LiveChartsTokenService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.3">Created</change>
// </history>

namespace Fanex.LiveCharts.ClientAuthentication
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using Fanex.LiveCharts.ClientAuthentication.Core;
    using Fanex.LiveCharts.ClientAuthentication.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LiveChartsTokenService
    {
        private const string RandomString = "1q2w3e4r!@#$";

        static LiveChartsTokenService()
        {
            LiveChartURL = GetConfigValue("LiveChartURL");
            LiveChartServiceURL = GetConfigValue("LiveChartServiceURL");
        }

        private static string LiveChartURL { get; set; }

        private static string LiveChartServiceURL { get; set; }

        /// <summary>
        /// Get url to access to live chart system.
        /// </summary>
        /// <param name="memberId">Current member id.</param>
        /// <param name="matchId">Match id need be get chart.</param>
        /// <param name="language">Language for globalization.</param>
        /// <returns>Url to access to live chart.</returns>
        public static string GetAccessUrl(int memberId, int matchId, int language = (int)LanguageType.English)
        {
            string encryptData = Cryptography.CfsEnCrypt(RandomString + memberId.ToString() + matchId.ToString());
            encryptData = HttpUtility.UrlEncode(encryptData);
            string getTokenString = string.Format(
                "{0}/api/authentication/gettoken?memberid={1}&encryptText={2}&matchid={3}",
                LiveChartServiceURL,
                memberId,
                encryptData,
                matchId);

            string tokenString = GetStream(getTokenString);

            if (string.IsNullOrEmpty(tokenString))
            {
                throw new Exception("Please re-check your input data.");
            }

            string tokenEncrypt = Cryptography.CfsEnCrypt(string.Format(
                "{0},{1}",
                JsonFormatter.Deserialize<string>(tokenString),
                memberId));

            string liveChartMatch = string.Format(
                "{0}/User/Authentication?matchid={1}&token={2}&language={3}",
                LiveChartURL,
                matchId,
                HttpUtility.UrlEncode(tokenEncrypt),
                language);

            return liveChartMatch;
        }

        /// <summary>
        /// Get List charts enable or disable.
        /// </summary>
        /// <param name="kickOffDate">Chart kickoff date.</param>
        /// <param name="timeZone">Current time zone to get data.</param>
        /// <param name="sportId">Sport Id information.</param>
        /// <param name="matchStatus">Match status for query.</param>
        /// <param name="chartShowingStatus">Chart can be show in production or both production and QA site.</param>
        /// <returns>List chart information.</returns>
        public static ChartInfo[] GetCharts(
            DateTime kickOffDate,
            int timeZone = -4,
            int sportId = 0,
            string matchStatus = "all",
            ChartShowingType chartShowingStatus = ChartShowingType.ShowOnProAndQASite)
        {
            string getCharUrl = string.Format(
                "{0}/api/authentication/getcharts?kickOffDate={1}&timeZone={2}&sportId={3}&matchStatus={4}&chartShowingStatus={5}",
                LiveChartServiceURL,
                kickOffDate.ToShortDateString(),
                timeZone,
                sportId,
                matchStatus,
                (byte)chartShowingStatus);

            string chartsStream = GetStream(getCharUrl);

            ChartInfo[] chartsData = JsonFormatter.Deserialize<ChartInfo[]>(chartsStream);

            return chartsData;
        }

        private static string GetConfigValue(string configName)
        {
            return ConfigurationManager.AppSettings[configName];
        }

        /// <summary>
        /// Get Stream form url request.
        /// </summary>
        /// <param name="urlRequest">Current url request.</param>
        /// <returns>Stream result.</returns>
        private static string GetStream(string urlRequest)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlRequest);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //// Get the stream associated with the response.
            Stream receiveStream = response.GetResponseStream();

            //// Pipes the stream to a higher level stream reader with the required encoding format.
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            var streamString = readStream.ReadToEnd();
            response.Close();
            readStream.Close();

            return streamString;
        }
    }
}