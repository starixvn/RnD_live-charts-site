// -----------------------------------------------------------------------
// <copyright file="GetEventsByMatchIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.7.24">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public class GetEventsByMatchIdCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public string CurrentLanguage
        {
            get
            {
                switch (Thread.CurrentThread.CurrentCulture.Name)
                {
                    case "zh-TW": return "cnt"; // Traditional
                    case "zh-CN": return "cn"; // Simplified
                    case "ja-JP": return "ja";
                    case "th-TH": return "th"; // Thai
                    case "ko-KR": return "ko"; // Korean
                    case "vi-VN": return "vi"; // Vietnamese
                    default: return string.Empty;
                }
            }
        }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Get_Event_By_Match_Id";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}