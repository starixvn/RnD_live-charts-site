// --------------------------------------------------------------------------
// <copyright file="LoadChart.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.7">Created</change>
// </history>

using System;
using Fanex.LiveCharts.ClientAuthentication;
using Fanex.LiveCharts.ClientAuthentication.Core;

/// <summary>
/// Load chart page.
/// </summary>
public partial class LoadChart : System.Web.UI.Page
{
    /// <summary>
    /// Page load information.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event Arguments.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string urlAccess = string.Empty;
        int currentMatchId = Convert.ToInt32(Request["matchId"]);
        if (!string.IsNullOrEmpty(Request["language"]))
        {
            LanguageType currentLanguage = LanguageType.English;
            switch (Request["language"])
            {
                case "zh-TW":
                    currentLanguage = LanguageType.ChineseTraditional;
                    break; // Traditional
                case "zh-CN":
                    currentLanguage = LanguageType.ChineseSimplified;
                    break; // Simplified
                case "ja-JP":
                    currentLanguage = LanguageType.Japanese;
                    break;
                case "th-TH":
                    currentLanguage = LanguageType.Thai;
                    break; // Thai
                case "ko-KR":
                    currentLanguage = LanguageType.Korean;
                    break; // Korean
                case "vi-VN":
                    currentLanguage = LanguageType.Vietnamese;
                    break; // Vietnamese
            }

            urlAccess = LiveChartsTokenService.GetAccessUrl(1, currentMatchId, (int)currentLanguage);
        }
        else
        {
            urlAccess = LiveChartsTokenService.GetAccessUrl(1, currentMatchId, (int)LanguageType.English);
        }

        Response.Redirect(urlAccess);
    }
}