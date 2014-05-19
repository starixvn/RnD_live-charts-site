// --------------------------------------------------------------------------
// <copyright file="UpdateBanMembersCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update ban for list of members.
    /// </summary>
    public class UpdateBanMembersCriteria : CriteriaBase
    {
        public string MemberIds { get; set; }

        public bool IsBanned { get; set; }

        public int InDatabase { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Update_Ban_Members";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(MemberIds) && InDatabase >= 0;
        }
    }
}