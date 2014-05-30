// --------------------------------------------------------------------------
// <copyright file="InsertBanWordsCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Insert Ban Words Criteria.
    /// </summary>
    public class InsertBanWordsCriteria : CriteriaBase
    {
        public string BanWords { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Insert_Ban_Words";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(BanWords);
        }
    }
}