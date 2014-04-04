// --------------------------------------------------------------------------
// <copyright file="GetChatRoomUsersCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.25">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetChatRoomUsersCriteria : CriteriaBase
    {
        public int ChatRoomId { get; set; }

        public bool IncludingBanned { get; set; }

        public bool IncludingKickedOut { get; set; }

        public bool IncludingLeft { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Chat_Room_Users";
        }

        public override bool IsValid()
        {
            return ChatRoomId > 0;
        }
    }
}