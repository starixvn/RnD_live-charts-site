// --------------------------------------------------------------------------
// <copyright file="UpdateBanWordTextCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class UpdateBanWordTextCriteria : CriteriaBase
    {
        public int BanWordId { get; set; }

        public string BanWordText { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Ban_Word_Text";
        }

        public override bool IsValid()
        {
            return BanWordId > 0;
        }
    }
}