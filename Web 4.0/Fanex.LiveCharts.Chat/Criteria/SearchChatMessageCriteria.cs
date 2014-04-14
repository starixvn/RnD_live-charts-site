// --------------------------------------------------------------------------
// <copyright file="SearchChatMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.14">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Search chat message criteria.
    /// </summary>
    public class SearchChatMessageCriteria : CriteriaBase
    {
        public int ChatRoomId { get; set; }

        public int ChatUserId { get; set; }

        public string SearchText { get; set; }

        public bool IncludingDeleted { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Search_Chat_Message";
        }

        public override bool IsValid()
        {
            return ChatRoomId > 0;
        }
    }
}