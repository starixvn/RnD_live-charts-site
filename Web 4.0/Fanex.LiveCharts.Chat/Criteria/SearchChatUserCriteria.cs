// --------------------------------------------------------------------------
// <copyright file="SearchChatUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.17">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchChatUserCriteria : CriteriaBase
    {
        public string ChatName { get; set; }

        public string MemberName { get; set; }

        public int ChatUserStatus { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Search_Chat_User";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}