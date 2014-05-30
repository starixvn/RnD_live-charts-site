// --------------------------------------------------------------------------
// <copyright file="GetBanWordsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.30">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetBanWordsCriteria : CriteriaBase
    {
        public string StartWith { get; set; }

        public string BanWordText { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Ban_Words";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}